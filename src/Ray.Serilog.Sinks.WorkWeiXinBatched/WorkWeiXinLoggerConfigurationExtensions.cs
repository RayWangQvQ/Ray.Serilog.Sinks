using System.Globalization;
using Ray.Serilog.Sinks.Batched;
using Serilog;
using Serilog.Configuration;
using Serilog.Events;

namespace Ray.Serilog.Sinks.WorkWeiXinBatched;

public static class WorkWeiXinLoggerConfigurationExtensions
{
    public static LoggerConfiguration WorkWeiXinBatched(
        this LoggerSinkConfiguration loggerSinkConfiguration,
        string webHookUrl,
        WorkWeiXinMsgType msgType = WorkWeiXinMsgType.text,
        bool sendBatchesAsOneMessages = true,
        int batchSizeLimit = int.MaxValue,
        string outputTemplate = Ray.Serilog.Sinks.Batched.Constants.DefaultOutputTemplate,
        IFormatProvider? formatProvider = null,
        LogEventLevel restrictedToMinimumLevel = LogEventLevel.Verbose
    )
    {
        if (loggerSinkConfiguration == null)
            throw new ArgumentNullException(nameof(loggerSinkConfiguration));
        if (outputTemplate == null)
            throw new ArgumentNullException(nameof(outputTemplate));

        return loggerSinkConfiguration.Sink(
            new WorkWeiXinBatchedSink(
                webHookUrl,
                msgType,
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
