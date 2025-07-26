using Ray.Serilog.Sinks.Batched;
using Serilog;
using Serilog.Configuration;
using Serilog.Events;
using System.Globalization;

namespace Ray.Serilog.Sinks.PushPlusBatched;

public static class PushPlusLoggerConfigurationExtensions
{
    public static LoggerConfiguration PushPlusBatched(
        this LoggerSinkConfiguration loggerSinkConfiguration,
        string token,
        string topic = "",
        string channel = "",
        string webhook = "",
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
            new PushPlusBatchedSink(
                token,
                topic,
                channel,
                webhook,
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
