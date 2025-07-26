namespace Ray.Serilog.Sinks.Batched;

public interface IPushService
{
    HttpResponseMessage PushMessage(string message, string title = "");
}
