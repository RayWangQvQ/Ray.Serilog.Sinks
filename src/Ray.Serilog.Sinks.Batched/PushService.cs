using Serilog.Debugging;

namespace Ray.Serilog.Sinks.Batched;

public abstract class PushService : IPushService
{
    protected abstract string ClientName { get; }

    protected virtual string? NewLineStr { get; }

    public virtual HttpResponseMessage PushMessage(string message, string title = "")
    {
        SelfLog.WriteLine($"开始推送到:{ClientName}");

        message = BuildMsg(message);

        return DoSend(message, title);
    }

    protected virtual string BuildMsg(string message, string title = "")
    {
        if (!string.IsNullOrEmpty(NewLineStr))
            return message.Replace(Environment.NewLine, NewLineStr);
        return message;
    }

    protected abstract HttpResponseMessage DoSend(string message, string title = "");
}
