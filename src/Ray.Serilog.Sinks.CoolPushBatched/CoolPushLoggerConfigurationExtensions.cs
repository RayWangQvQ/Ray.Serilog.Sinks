using System.Globalization;
using Ray.Serilog.Sinks.Batched;
using Serilog;
using Serilog.Configuration;
using Serilog.Events;

namespace Ray.Serilog.Sinks.CoolPushBatched;

public static class CoolPushLoggerConfigurationExtensions
{
    public static LoggerConfiguration CoolPushBatched(
        this LoggerSinkConfiguration loggerSinkConfiguration,
        string sKey,
        bool sendBatchesAsOneMessages = true,
        int batchSizeLimit = int.MaxValue,
        IFormatProvider? formatProvider = null,
        LogEventLevel restrictedToMinimumLevel = LogEventLevel.Verbose
    )
    {
        if (loggerSinkConfiguration == null)
            throw new ArgumentNullException(nameof(loggerSinkConfiguration));

        return loggerSinkConfiguration.Sink(
            new CoolPushBatchedSink(
                sKey,
                sendBatchesAsOneMessages,
                batchSizeLimit,
                formatProvider ?? CultureInfo.InvariantCulture,
                restrictedToMinimumLevel
            ),
            restrictedToMinimumLevel
        );
    }
}
