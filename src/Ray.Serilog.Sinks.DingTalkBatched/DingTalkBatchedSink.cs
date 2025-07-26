using Ray.Serilog.Sinks.Batched;
using Serilog.Events;

namespace Ray.Serilog.Sinks.DingTalkBatched;

public class DingTalkBatchedSink : BatchedSink
{
    private readonly string _webHookUrl;

    public DingTalkBatchedSink(
        string webHookUrl,
        bool sendBatchesAsOneMessages,
        int batchSizeLimit,
        IFormatProvider formatProvider,
        LogEventLevel minimumLogEventLevel
    )
        : base(sendBatchesAsOneMessages, batchSizeLimit, formatProvider: formatProvider, minimumLogEventLevel: minimumLogEventLevel)
    {
        _webHookUrl = webHookUrl;
    }

    public override void Emit(LogEvent logEvent)
    {
        if (_webHookUrl.IsNullOrEmpty())
            return;
        base.Emit(logEvent);
    }

    protected override PushService PushService => new DingTalkApiClient(_webHookUrl);
}
