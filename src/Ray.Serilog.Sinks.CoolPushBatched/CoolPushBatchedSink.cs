using Ray.Serilog.Sinks.Batched;
using Serilog.Events;

namespace Ray.Serilog.Sinks.CoolPushBatched;

public class CoolPushBatchedSink : BatchedSink
{
    private readonly string _sKey;

    public CoolPushBatchedSink(
        string sKey,
        bool sendBatchesAsOneMessages,
        int batchSizeLimit,
        IFormatProvider formatProvider,
        LogEventLevel minimumLogEventLevel
    )
        : base(sendBatchesAsOneMessages, batchSizeLimit, formatProvider: formatProvider, minimumLogEventLevel: minimumLogEventLevel)
    {
        _sKey = sKey;
    }

    public override void Emit(LogEvent logEvent)
    {
        if (_sKey.IsNullOrEmpty())
            return;
        base.Emit(logEvent);
    }

    protected override PushService PushService => new CoolPushApiClient(_sKey);
}
