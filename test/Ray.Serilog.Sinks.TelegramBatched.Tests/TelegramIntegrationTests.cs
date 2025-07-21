using FluentAssertions;
using Serilog;

namespace Ray.Serilog.Sinks.TelegramBatched.Tests;

public class TelegramIntegrationTests
{
    [Fact]
    public void Logger_WithTelegramSink_ShouldCreateSuccessfully()
    {
        // Arrange & Act
        var logger = new LoggerConfiguration()
            .WriteTo.TelegramBatched(
                "test_token",
                "test_chat_id",
                "")
            .CreateLogger();

        // Assert
        logger.Should().NotBeNull();
    }

    [Fact]
    public void Logger_WriteInformation_ShouldNotThrow()
    {
        // Arrange
        var logger = new LoggerConfiguration()
            .WriteTo.TelegramBatched(
                "test_token",
                "test_chat_id",
                "")
            .CreateLogger();

        // Act & Assert
        logger.Invoking(l => l.Information("Test message")).Should().NotThrow();
    }

    [Fact]
    public void Logger_WriteWithTrigger_ShouldNotThrow()
    {
        // Arrange
        var logger = new LoggerConfiguration()
            .WriteTo.TelegramBatched(
                "test_token",
                "test_chat_id",
                "",
                "【Push】")
            .CreateLogger();

        // Act & Assert
        logger.Invoking(l => l.Information("【Push】Test message")).Should().NotThrow();
    }
}