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
            "【Push】",
            false,
            null,
            LogEventLevel.Warning
        );

        // Assert
        result.Should().NotBeNull();
    }

    [Fact]
    public void TelegramBatched_WithNullContainsTrigger_ShouldUseDefault()
    {
        // Arrange
        var loggerConfiguration = new LoggerConfiguration();

        // Act
        var result = loggerConfiguration.WriteTo.TelegramBatched(
            TestBotToken,
            TestChatId,
            "",
            null
        );

        // Assert
        result.Should().NotBeNull();
    }

    [Fact]
    public void TelegramBatched_WithEmptyContainsTrigger_ShouldUseDefault()
    {
        // Arrange
        var loggerConfiguration = new LoggerConfiguration();

        // Act
        var result = loggerConfiguration.WriteTo.TelegramBatched(TestBotToken, TestChatId, "", "");

        // Assert
        result.Should().NotBeNull();
    }
}
