using Ray.Serilog.Sinks.Batched;
using Ray.Serilog.Sinks.TelegramBatched;
using Serilog;
using Serilog.Context;

Console.WriteLine("Hello, World!");

var logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.TelegramBatched("YOUR_BOT_TOKEN", "YOUR_CHAT_ID")
    .CreateLogger();
Log.Logger = logger;

var groupId = "test-group";

using (LogContext.PushProperty(Ray.Serilog.Sinks.Batched.Constants.GroupPropertyKey, groupId))
{
    Log.Information("This is a test log message.");
}

await BatchSinkManager.FlushAsync(groupId);
