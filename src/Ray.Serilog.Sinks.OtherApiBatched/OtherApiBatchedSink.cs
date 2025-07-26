using Ray.Serilog.Sinks.Batched;
using Serilog.Events;

namespace Ray.Serilog.Sinks.OtherApiBatched;

public class OtherApiBatchedSink : BatchedSink
{
    private readonly string _api;
    private readonly string _jsonTemplate;
    private readonly string _placeholder;

    public OtherApiBatchedSink(
        string api,
        string jsonTemplate,
        string placeholder,
        bool sendBatchesAsOneMessages,
        int batchSizeLimit,
        IFormatProvider formatProvider,
        LogEventLevel minimumLogEventLevel
    )
        : base(sendBatchesAsOneMessages, batchSizeLimit, null, formatProvider, minimumLogEventLevel)
    {
        _api = api;
        _jsonTemplate = jsonTemplate;
        _placeholder = placeholder;
    }

    public override void Emit(LogEvent logEvent)
    {
        if (_api.IsNullOrEmpty())
            return;
        base.Emit(logEvent);
    }

    protected override PushService PushService =>
        new OtherApiClient(_api, _jsonTemplate, _placeholder);
}
