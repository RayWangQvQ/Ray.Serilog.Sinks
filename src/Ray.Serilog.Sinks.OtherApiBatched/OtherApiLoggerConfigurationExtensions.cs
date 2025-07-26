using Serilog;
using Serilog.Configuration;
using Serilog.Events;
using System.Globalization;

namespace Ray.Serilog.Sinks.OtherApiBatched;

public static class OtherApiLoggerConfigurationExtensions
{
    public static LoggerConfiguration OtherApiBatched(
        this LoggerSinkConfiguration loggerSinkConfiguration,
        string api,
        string bodyJsonTemplate,
        string placeholder,
        bool sendBatchesAsOneMessages = true,
        int batchSizeLimit = int.MaxValue,
        IFormatProvider? formatProvider = null,
        LogEventLevel restrictedToMinimumLevel = LogEventLevel.Verbose
    )
    {
        if (loggerSinkConfiguration == null)
            throw new ArgumentNullException(nameof(loggerSinkConfiguration));

        return loggerSinkConfiguration.Sink(
            new OtherApiBatchedSink(
                api,
                bodyJsonTemplate,
                placeholder,
                sendBatchesAsOneMessages,
                batchSizeLimit,
                formatProvider ?? CultureInfo.InvariantCulture,
                restrictedToMinimumLevel
            ),
            restrictedToMinimumLevel
        );
    }
}
