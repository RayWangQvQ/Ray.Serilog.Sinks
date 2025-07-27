# Ray.Serilog.Sinks

A collection of batched push Sinks for Serilog, supporting batch sending of logs to various notification services and APIs.

## 🚀 Features

- **Batch Processing**: Supports batch sending of multiple log messages for improved performance
- **Multi-Platform Support**: Compatible with .NET 6.0, .NET 7.0, and .NET 8.0
- **Rich Sinks**: Supports various popular notification and messaging platforms
- **Flexible Configuration**: Provides rich configuration options to meet different scenario requirements
- **Group Management**: Supports batch flushing of logs by group
- **Asynchronous Operations**: Fully asynchronous operations that don't block the main thread

## 📦 Supported Sinks

| Sink | Description | NuGet Package |
|------|-------------|---------------|
| **DingTalkBatched** | DingTalk robot notifications | `Ray.Serilog.Sinks.DingTalkBatched` |
| **TelegramBatched** | Telegram bot notifications | `Ray.Serilog.Sinks.TelegramBatched` |
| **MicrosoftTeamsBatched** | Microsoft Teams notifications | `Ray.Serilog.Sinks.MicrosoftTeamsBatched` |
| **WorkWeiXinBatched** | Enterprise WeChat group robot | `Ray.Serilog.Sinks.WorkWeiXinBatched` |
| **WorkWeiXinAppBatched** | Enterprise WeChat app messages | `Ray.Serilog.Sinks.WorkWeiXinAppBatched` |
| **ServerChanBatched** | ServerChan push notifications | `Ray.Serilog.Sinks.ServerChanBatched` |
| **PushPlusBatched** | PushPlus push notifications | `Ray.Serilog.Sinks.PushPlusBatched` |
| **CoolPushBatched** | CoolPush notifications | `Ray.Serilog.Sinks.CoolPushBatched` |
| **GotifyBatched** | Gotify self-hosted push | `Ray.Serilog.Sinks.GotifyBatched` |
| **OtherApiBatched** | Custom API push | `Ray.Serilog.Sinks.OtherApiBatched` |

## 📖 Installation

### Using NuGet Package Manager

```bash
# Install base package
Install-Package Ray.Serilog.Sinks.Batched

# Install specific Sink (Telegram example)
Install-Package Ray.Serilog.Sinks.TelegramBatched
```

### Using .NET CLI

```bash
# Install base package
dotnet add package Ray.Serilog.Sinks.Batched

# Install specific Sink
dotnet add package Ray.Serilog.Sinks.TelegramBatched
```

## 🔧 Basic Usage

### Simple Configuration

```csharp
using Ray.Serilog.Sinks.Batched;
using Ray.Serilog.Sinks.TelegramBatched;
using Serilog;
using Serilog.Context;

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.TelegramBatched("YOUR_BOT_TOKEN", "YOUR_CHAT_ID")
    .CreateLogger();

// Log messages
Log.Information("This is a test log message");
```

### Group Batch Processing

```csharp
using Ray.Serilog.Sinks.Batched;
using Serilog.Context;

// Use groups for batch processing
var groupId = "error-notifications";

using (LogContext.PushProperty(Constants.GroupPropertyKey, groupId))
{
    Log.Error("An error occurred");
    Log.Error("Another error occurred");
    Log.Warning("This is a warning");
}

// Batch flush logs for the specified group
await BatchSinkManager.FlushAsync(groupId);
```

## 🎯 Detailed Configuration Examples

### Telegram Bot

```csharp
Log.Logger = new LoggerConfiguration()
    .WriteTo.TelegramBatched(
        botToken: "YOUR_BOT_TOKEN",
        chatId: "YOUR_CHAT_ID",
        proxy: "", // Optional proxy settings
        sendBatchesAsOneMessages: true, // Whether to send batch messages as one
        batchSizeLimit: 50, // Batch size limit
        restrictedToMinimumLevel: LogEventLevel.Warning // Minimum log level
    )
    .CreateLogger();
```

### DingTalk Robot

```csharp
Log.Logger = new LoggerConfiguration()
    .WriteTo.DingTalkBatched(
        webHookUrl: "YOUR_DINGTALK_WEBHOOK_URL",
        sendBatchesAsOneMessages: true,
        batchSizeLimit: 100,
        restrictedToMinimumLevel: LogEventLevel.Information
    )
    .CreateLogger();
```

### Enterprise WeChat Group Robot

```csharp
Log.Logger = new LoggerConfiguration()
    .WriteTo.WorkWeiXinBatched(
        webHookUrl: "YOUR_WORK_WEIXIN_WEBHOOK_URL",
        sendBatchesAsOneMessages: true,
        batchSizeLimit: 50
    )
    .CreateLogger();
```

### Multiple Sinks Combined

```csharp
Log.Logger = new LoggerConfiguration()
    .WriteTo.TelegramBatched("BOT_TOKEN", "CHAT_ID")
    .WriteTo.DingTalkBatched("DINGTALK_WEBHOOK")
    .WriteTo.Console() // Also output to console
    .CreateLogger();
```

## 🛠️ Advanced Features

### Custom Output Template

```csharp
Log.Logger = new LoggerConfiguration()
    .WriteTo.TelegramBatched(
        botToken: "YOUR_BOT_TOKEN",
        chatId: "YOUR_CHAT_ID",
        outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss}] [{Level:u3}] {Message:lj}{NewLine}{Exception}"
    )
    .CreateLogger();
```

### Conditional Logging

```csharp
Log.Logger = new LoggerConfiguration()
    .WriteTo.Logger(lc => lc
        .Filter.ByIncludingOnly(evt => evt.Level >= LogEventLevel.Error)
        .WriteTo.TelegramBatched("BOT_TOKEN", "ERROR_CHAT_ID")
    )
    .WriteTo.Logger(lc => lc
        .Filter.ByIncludingOnly(evt => evt.Level >= LogEventLevel.Warning)
        .WriteTo.DingTalkBatched("DINGTALK_WEBHOOK")
    )
    .CreateLogger();
```

## 📝 Configuration Parameters

### Common Parameters

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `sendBatchesAsOneMessages` | `bool` | `true` | Whether to merge batch messages into one |
| `batchSizeLimit` | `int` | `int.MaxValue` | Batch processing size limit |
| `restrictedToMinimumLevel` | `LogEventLevel` | `Verbose` | Minimum log level |
| `formatProvider` | `IFormatProvider` | `null` | Format provider |

### Specific Parameters

Different Sinks may have specific configuration parameters. Please refer to their respective documentation.

## 🔍 Troubleshooting

### Common Issues

1. **Messages Not Sent**
   - Check network connectivity
   - Verify API keys and configuration
   - Confirm log level settings

2. **Batch Message Issues**
   - Adjust `batchSizeLimit` parameter
   - Check `sendBatchesAsOneMessages` setting
   - Ensure `BatchSinkManager.FlushAsync()` is called

3. **Performance Issues**
   - Set appropriate batch size limits
   - Consider using asynchronous logging
   - Monitor network request frequency

## 🤝 Contributing

Issues and Pull Requests are welcome!

1. Fork this repository
2. Create a feature branch
3. Commit your changes
4. Push to the branch
5. Create a Pull Request

## 📄 License

This project is licensed under the [GNU General Public License v3](LICENSE).

## 🔗 Related Links

- [Serilog Official Documentation](https://serilog.net/)
- [Project Changelog](CHANGELOG.md)

## 👨‍💻 Author

- **Ray** - Project Maintainer

---

If this project helps you, please give it a ⭐ Star!
