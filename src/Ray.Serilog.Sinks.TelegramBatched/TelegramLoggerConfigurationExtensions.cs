using Ray.Serilog.Sinks.Batched;
using Serilog;
using Serilog.Configuration;
using Serilog.Events;
using System.Globalization;

namespace Ray.Serilog.Sinks.TelegramBatched;

public static class TelegramLoggerConfigurationExtensions
{
    public static LoggerConfiguration TelegramBatched(
        this LoggerSinkConfiguration loggerSinkConfiguration,
        string botToken,
        string chatId,
        string proxy = "",
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
            new TelegramBatchedSink(
                botToken,
                chatId,
                proxy,
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
