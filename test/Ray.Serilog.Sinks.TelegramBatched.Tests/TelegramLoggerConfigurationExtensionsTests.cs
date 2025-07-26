using FluentAssertions;
using Serilog;
using Serilog.Events;

namespace Ray.Serilog.Sinks.TelegramBatched.Tests;

public class TelegramLoggerConfigurationExtensionsTests
{
    private const string TestBotToken = "123456789:ABCDEFGHIJKLMNOP-qrstuvwxyz";
    private const string TestChatId = "-1001234567890";

    [Fact]
    public void TelegramBatched_WithValidParameters_ShouldConfigureLogger()
    {
        // Arrange
        var loggerConfiguration = new LoggerConfiguration();

        // Act
        var result = loggerConfiguration.WriteTo.TelegramBatched(TestBotToken, TestChatId, "");

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<LoggerConfiguration>();
    }

    [Fact]
    public void TelegramBatched_WithAllParameters_ShouldConfigureLogger()
    {
        // Arrange
        var loggerConfiguration = new LoggerConfiguration();

        // Act
        var result = loggerConfiguration.WriteTo.TelegramBatched(
            TestBotToken,
            TestChatId,
            "http://proxy.example.com:8080",
            false,
            50,
            "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
            null,
            LogEventLevel.Warning
        );

        // Assert
        result.Should().NotBeNull();
    }

    [Fact]
    public void TelegramBatched_WithMinimalParameters_ShouldUseDefaults()
    {
        // Arrange
        var loggerConfiguration = new LoggerConfiguration();

        // Act
        var result = loggerConfiguration.WriteTo.TelegramBatched(
            TestBotToken,
            TestChatId
        );

        // Assert
        result.Should().NotBeNull();
    }
}
