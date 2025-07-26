using Ray.Serilog.Sinks.Batched;
using Serilog.Events;

namespace Ray.Serilog.Sinks.MicrosoftTeamsBatched;

public class MicrosoftTeamsBatchedSink : BatchedSink
{
    private readonly string _webhook;

    public MicrosoftTeamsBatchedSink(
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
        _webhook = webhook;
    }

    public override void Emit(LogEvent logEvent)
    {
        if (_webhook.IsNullOrEmpty())
            return;
        base.Emit(logEvent);
    }

    protected override PushService PushService => new MicrosoftTeamsApiClient(webhook: _webhook);
}
