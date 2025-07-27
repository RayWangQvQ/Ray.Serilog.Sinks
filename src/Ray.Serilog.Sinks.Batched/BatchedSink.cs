using System.Collections.Concurrent;
using System.Text;
using Serilog.Core;
using Serilog.Debugging;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Formatting.Display;

namespace Ray.Serilog.Sinks.Batched;

public abstract class BatchedSink : ILogEventSink, IDisposable, IBatchSink
{
    private readonly ConcurrentDictionary<string, ConcurrentQueue<LogEvent>> _groupLogEvents =
        new();

    private readonly int _batchSizeLimit;
    private readonly LogEventLevel _minimumLogEventLevel;
    private readonly bool _sendBatchesAsOneMessages;
    private readonly ITextFormatter _formatter;
    private readonly object _syncRoot = new();
    private readonly SemaphoreSlim _flushSemaphore;
    private bool _disposed;

    protected BatchedSink(
        bool sendBatchesAsOneMessages,
        int batchSizeLimit,
        IFormatProvider formatProvider,
        LogEventLevel minimumLogEventLevel
    )
        : this(sendBatchesAsOneMessages, batchSizeLimit, null, formatProvider, minimumLogEventLevel)
    { }

    protected BatchedSink(
        bool sendBatchesAsOneMessages = true,
        int batchSizeLimit = int.MaxValue,
        string? outputTemplate = "{Message:lj}{NewLine}{Exception}",
        IFormatProvider? formatProvider = null,
        LogEventLevel minimumLogEventLevel = LogEventLevel.Verbose
    )
    {
        _minimumLogEventLevel = minimumLogEventLevel;
        _sendBatchesAsOneMessages = sendBatchesAsOneMessages;
        _batchSizeLimit = batchSizeLimit;

        outputTemplate = string.IsNullOrWhiteSpace(outputTemplate)
            ? Constants.DefaultOutputTemplate
            : outputTemplate;
        _formatter = new MessageTemplateTextFormatter(outputTemplate, formatProvider);

        _flushSemaphore = new SemaphoreSlim(1, 1);

        BatchSinkManager.RegisterSink(this);
    }

    public virtual void Emit(LogEvent logEvent)
    {
        if (_disposed)
        {
            return;
        }

        if (logEvent == null)
        {
            throw new ArgumentNullException(nameof(logEvent));
        }

        try
        {
            if (logEvent.Level < _minimumLogEventLevel)
            {
                return;
            }

            var context =
                JobContextManager.Current ?? new JobExecutionContext("Unknown", "UnknownJob");

            var enrichedProperties = logEvent
                .Properties.Select(kvp => new LogEventProperty(kvp.Key, kvp.Value))
                .ToList();
            enrichedProperties.Add(new LogEventProperty("JobId", new ScalarValue(context.JobId)));
            enrichedProperties.Add(
                new LogEventProperty("JobName", new ScalarValue(context.JobName))
            );

            var enrichedEvent = new LogEvent(
                logEvent.Timestamp,
                logEvent.Level,
                logEvent.Exception,
                logEvent.MessageTemplate,
                enrichedProperties
            );

            var queue = _groupLogEvents.GetOrAdd(
                context.JobId,
                _ => new ConcurrentQueue<LogEvent>()
            );
            queue.Enqueue(enrichedEvent);

            if (queue.Count <= _batchSizeLimit)
                return;

            SelfLog.WriteLine(
                "BatchedSink queue size exceeded limit ({0} > {1}), flushing immediately.",
                queue.Count,
                _batchSizeLimit
            );
            Task.Run(async () => await FlushAsync(context.JobId).ConfigureAwait(false));
        }
        catch (Exception ex)
        {
            SelfLog.WriteLine(
                "Exception while emitting periodic batch from {0}: {1}",
                this,
                ex.Message
            );
        }
    }

    public async Task FlushAsync(string jobId, string title = "")
    {
        if (_disposed)
            return;

        await _flushSemaphore.WaitAsync();

        try
        {
            if (_groupLogEvents.TryGetValue(jobId, out var queue))
            {
                var batch = new List<LogEvent>();

                while (queue.TryDequeue(out var logEvent) && batch.Count < _batchSizeLimit)
                {
                    batch.Add(logEvent);
                }

                if (batch.Count > 0)
                {
                    await EmitBatchAsync(batch, jobId, title);
                }

                if (queue.IsEmpty)
                {
                    _groupLogEvents.TryRemove(jobId, out _);
                }
            }
        }
        finally
        {
            _flushSemaphore.Release();
        }
    }

    public async Task FlushAllAsync(string title = "")
    {
        if (_disposed)
            return;

        var jobIds = _groupLogEvents.Keys.ToList();
        var tasks = jobIds.Select(x => FlushAsync(x, title));
        await Task.WhenAll(tasks);
    }

    protected virtual async Task EmitBatchAsync(
        IEnumerable<LogEvent> events,
        string jobId,
        string title = ""
    )
    {
        if (_disposed)
        {
            return;
        }

        if (_sendBatchesAsOneMessages)
        {
            var sb = new StringBuilder();
            foreach (var logEvent in events)
            {
                string message = RenderMessage(logEvent);
                sb.Append(message);
            }
            sb.AppendLine(Environment.NewLine);

            var messageToSend = sb.ToString();
            await PushMessageAsync(messageToSend, title);
        }
        else
        {
            foreach (var logEvent in events)
            {
                var message = RenderMessage(logEvent);
                await PushMessageAsync(message);
            }
        }
    }

    protected abstract IPushService PushService { get; }

    public bool IsDisposed => _disposed;

    protected virtual async Task PushMessageAsync(string message, string title = "推送")
    {
        //SelfLog.WriteLine($"Trying to send message: '{message}'.");
        var result = await PushService.PushMessageAsync(message, title);
        SelfLog.WriteLine($"Response status: {result.StatusCode}.");
        try
        {
            var content = (await result.Content.ReadAsStringAsync())
                .Replace("{", "{{")
                .Replace("}", "}}");
            SelfLog.WriteLine($"Response content: {content}.{Environment.NewLine}");
        }
        catch (Exception e)
        {
            SelfLog.WriteLine(e.Message + Environment.NewLine);
        }
    }

    protected virtual string RenderMessage(LogEvent logEvent)
    {
        string msg = "";
        using (StringWriter stringWriter = new StringWriter())
        {
            this._formatter.Format(logEvent, (TextWriter)stringWriter);
            msg = stringWriter.ToString();
        }

        //msg = $"{GetEmoji(logEvent)} {msg}";

        if (msg.Contains("经验+") && msg.Contains("√"))
            msg = msg.Replace('√', '✔');

        return msg;

        /*
        if (logEvent.Exception == null)
        {
            return msg;
        }

        var sb = new StringBuilder();
        sb.AppendLine(msg);
        sb.AppendLine($"\n*{logEvent.Exception.Message}*\n");
        sb.AppendLine($"Message: `{logEvent.Exception.Message}`");
        sb.AppendLine($"Type: `{logEvent.Exception.GetType().Name}`\n");
        sb.AppendLine($"Stack Trace\n```{logEvent.Exception}```");

        return sb.ToString();
        */
    }

    protected virtual string GetEmoji(LogEvent log)
    {
        switch (log.Level)
        {
            case LogEventLevel.Verbose:
                return "⚡";
            case LogEventLevel.Debug:
                return "👉";
            case LogEventLevel.Information:
                return "ℹ";
            case LogEventLevel.Warning:
                return "⚠";
            case LogEventLevel.Error:
                return "❗";
            case LogEventLevel.Fatal:
                return "‼";
            default:
                return string.Empty;
        }
    }

    protected virtual string GetPushTitle(LogEvent triggerLogEvent)
    {
        var title = "推送";

        var msg = RenderMessage(triggerLogEvent).Replace(Environment.NewLine, "");
        var list = msg.Split('·').ToList();

        for (int i = 2; i < list.Count; i++)
        {
            if (!string.IsNullOrWhiteSpace(list[i]))
                title += $"-{list[i]}";
        }

        return title;
    }

    public virtual void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        lock (_syncRoot)
        {
            if (_disposed)
                return;
        }

        try
        {
            FlushAllAsync().ConfigureAwait(false).GetAwaiter().GetResult();
        }
        catch (Exception ex)
        {
            SelfLog.WriteLine("Exception while disposing BatchedSink: {0}", ex.Message);
        }

        BatchSinkManager.UnregisterSink(this);
        _flushSemaphore.Dispose();

        _disposed = true;
    }
}
