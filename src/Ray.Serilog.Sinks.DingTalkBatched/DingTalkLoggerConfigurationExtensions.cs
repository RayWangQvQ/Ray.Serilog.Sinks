using System.Globalization;
using Serilog;
using Serilog.Configuration;
using Serilog.Events;

namespace Ray.Serilog.Sinks.DingTalkBatched;

public static class DingTalkLoggerConfigurationExtensions
{
    public static LoggerConfiguration DingTalkBatched(
        this LoggerSinkConfiguration loggerSinkConfiguration,
        string webHookUrl,
        bool sendBatchesAsOneMessages = true,
        int batchSizeLimit = int.MaxValue,
        IFormatProvider? formatProvider = null,
        LogEventLevel restrictedToMinimumLevel = LogEventLevel.Verbose
    )
    {
        return loggerSinkConfiguration.Sink(
            new DingTalkBatchedSink(
                webHookUrl,
                sendBatchesAsOneMessages,
                batchSizeLimit,
                formatProvider ?? CultureInfo.InvariantCulture,
                restrictedToMinimumLevel
            ),
            restrictedToMinimumLevel
        );
    }
}
