using System.Globalization;
using Ray.Serilog.Sinks.Batched;
using Serilog;
using Serilog.Configuration;
using Serilog.Events;

namespace Ray.Serilog.Sinks.WorkWeiXinAppBatched;

public static class WorkWeiXinAppConfigurationExtensions
{
    public static LoggerConfiguration WorkWeiXinAppBatched(
        this LoggerSinkConfiguration loggerSinkConfiguration,
        string corpId,
        string agentId,
        string secret,
        string toUser = "",
        string toParty = "",
        string toTag = "",
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
            new WorkWeiXinAppBatchedSink(
                corpId,
                agentId,
                secret,
                toUser,
                toParty,
                toTag,
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
