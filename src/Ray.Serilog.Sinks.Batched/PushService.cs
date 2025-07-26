using Serilog.Debugging;

namespace Ray.Serilog.Sinks.Batched;

public abstract class PushService
{
    /// <summary>
    /// 推送端/推送平台名称
    /// </summary>
    protected abstract string ClientName { get; }

    protected string Msg { get; set; }

    protected string Title { get; set; }

    protected virtual string NewLineStr { get; }

    public virtual HttpResponseMessage PushMessage(string message, string title = "")
    {
        Msg = message;
        Title = title;

        SelfLog.WriteLine($"开始推送到:{ClientName}");

        BuildMsg();

        return DoSend();
    }

    /// <summary>
    /// 构建消息
    /// </summary>
    /// <returns></returns>
    public virtual void BuildMsg()
    {
        //如果指定换行符，则替换；不指定，不替换
        if (!string.IsNullOrEmpty(NewLineStr))
            Msg = Msg.Replace(Environment.NewLine, this.NewLineStr);
    }

    /// <summary>
    /// 实际发送
    /// </summary>
    /// <returns></returns>
    protected abstract HttpResponseMessage DoSend();
}
