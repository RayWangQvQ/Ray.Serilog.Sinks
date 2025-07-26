using System.Globalization;
using Ray.Serilog.Sinks.Batched;
using Serilog;
using Serilog.Configuration;
using Serilog.Events;

namespace Ray.Serilog.Sinks.ServerChanBatched;

public static class ServerChanLoggerConfigurationExtensions
{
    public static LoggerConfiguration ServerChanBatched(
        this LoggerSinkConfiguration loggerSinkConfiguration,
        string scKey,
        string turboScKey = "",
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
            new ServerChanBatchedSink(
                scKey,
                turboScKey,
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
