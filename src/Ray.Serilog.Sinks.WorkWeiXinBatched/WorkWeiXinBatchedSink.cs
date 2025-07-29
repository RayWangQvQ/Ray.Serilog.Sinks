using Ray.Serilog.Sinks.Batched;
using Serilog.Events;

namespace Ray.Serilog.Sinks.WorkWeiXinBatched;

public class WorkWeiXinBatchedSink : BatchedSink
{
    private readonly string _webHookUrl;
    private readonly WorkWeiXinMsgType _msgType;

    public WorkWeiXinBatchedSink(
        string webHookUrl,
        WorkWeiXinMsgType msgType,
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
        _webHookUrl = webHookUrl;
        _msgType = msgType;
    }

    public override void Emit(LogEvent logEvent)
    {
        if (_webHookUrl.IsNullOrEmpty())
            return;
        base.Emit(logEvent);
    }

    protected override PushService PushService => new WorkWeiXinApiClient(_webHookUrl, _msgType);
}
