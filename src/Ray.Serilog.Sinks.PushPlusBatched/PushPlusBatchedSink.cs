using Ray.Serilog.Sinks.Batched;
using Serilog.Events;

namespace Ray.Serilog.Sinks.PushPlusBatched;

public class PushPlusBatchedSink : BatchedSink
{
    private readonly string _token;
    private readonly string _topic;
    private readonly string _channel;
    private readonly string _webhook;

    public PushPlusBatchedSink(
        string token,
        string topic,
        string channel,
        string webhook,
        bool sendBatchesAsOneMessages,
        int batchSizeLimit,
        string outputTemplate,
        IFormatProvider formatProvider,
        LogEventLevel minimumLogEventLevel
    )
        : base(
            sendBatchesAsOneMessages,
            batchSizeLimit,
            outputTemplate: outputTemplate,
            formatProvider: formatProvider,
            minimumLogEventLevel: minimumLogEventLevel
        )
    {
        _token = token;
        _topic = topic;
        _channel = channel;
        _webhook = webhook;
    }

    public override void Emit(LogEvent logEvent)
    {
        if (_token.IsNullOrEmpty())
            return;
        base.Emit(logEvent);
    }

    protected override PushService PushService =>
        new PushPlusApiClient(_token, _topic, channel: _channel, webhook: _webhook);
}
