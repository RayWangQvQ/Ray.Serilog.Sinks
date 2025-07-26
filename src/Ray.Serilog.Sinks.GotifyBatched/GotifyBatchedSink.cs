using Ray.Serilog.Sinks.Batched;
using Serilog.Events;

namespace Ray.Serilog.Sinks.GotifyBatched;

public class GotifyBatchedSink : BatchedSink
{
    private readonly string _host;
    private readonly string _token;

    public GotifyBatchedSink(
        string host,
        string token,
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
        _host = host;
        _token = token;
    }

    public override void Emit(LogEvent logEvent)
    {
        if (_host.IsNullOrEmpty() || _token.IsNullOrEmpty())
            return;
        base.Emit(logEvent);
    }

    protected override PushService PushService => new GotifyApiClient(_host, _token);
}
