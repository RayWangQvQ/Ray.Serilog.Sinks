using System.Globalization;
using Ray.Serilog.Sinks.Batched;
using Serilog;
using Serilog.Configuration;
using Serilog.Events;

namespace Ray.Serilog.Sinks.GotifyBatched;

public static class GotifyConfigurationExtensions
{
    public static LoggerConfiguration GotifyBatched(
        this LoggerSinkConfiguration loggerSinkConfiguration,
        string host,
        string token,
        bool sendBatchesAsOneMessages = true,
        int batchSizeLimit = int.MaxValue,
        string outputTemplate = Constants.DefaultOutputTemplate,
        IFormatProvider? formatProvider = null,
        LogEventLevel restrictedToMinimumLevel = LogEventLevel.Verbose
    )
    {
        if (loggerSinkConfiguration == null)
            throw new ArgumentNullException(nameof(loggerSinkConfiguration));
        if (outputTemplate == null)
            throw new ArgumentNullException(nameof(outputTemplate));

        return loggerSinkConfiguration.Sink(
            new GotifyBatchedSink(
                host,
                token,
                sendBatchesAsOneMessages,
                batchSizeLimit,
                outputTemplate,
                formatProvider ?? CultureInfo.InvariantCulture,
                restrictedToMinimumLevel
            ),
            restrictedToMinimumLevel
        );
    }
}
