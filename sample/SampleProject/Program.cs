using Ray.Serilog.Sinks.Batched;
using Ray.Serilog.Sinks.DingTalkBatched;
using Ray.Serilog.Sinks.TelegramBatched;
using Ray.Serilog.Sinks.WorkWeiXinAppBatched;
using Ray.Serilog.Sinks.WorkWeiXinBatched;
using Serilog;
using Serilog.Context;
using Serilog.Debugging;

Console.WriteLine("Hello, World!");

SelfLog.Enable(x => Console.WriteLine(x ?? ""));
Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.Debug()
    .WriteTo.TelegramBatched("YOUR_BOT_TOKEN", "YOUR_CHAT_ID")
    .WriteTo.DingTalkBatched("https://oapi.dingtalk.com/robot/send?access_token=abcd")
    .WriteTo.WorkWeiXinAppBatched("corpid", "agentid", "secret")
    .WriteTo.WorkWeiXinBatched("https://qyapi.weixin.qq.com/cgi-bin/webhook/send?key=abcd")
    .CreateLogger();

var groupId = "test-group";
using (LogContext.PushProperty(Ray.Serilog.Sinks.Batched.Constants.GroupPropertyKey, groupId))
{
    Log.Information("This is a test log message.");
}
await BatchSinkManager.FlushAsync(groupId, "");

groupId = "test-group2";
using (LogContext.PushProperty(Ray.Serilog.Sinks.Batched.Constants.GroupPropertyKey, groupId))
{
    Log.Information("This is a test log message 1.");
    Log.Information("This is a test log message 2.");
}
await BatchSinkManager.FlushAsync(groupId);
