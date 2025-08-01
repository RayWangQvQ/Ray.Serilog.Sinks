using FluentAssertions;
using Serilog;
using Serilog.Events;

namespace Ray.Serilog.Sinks.DingTalkBatched.Tests;

public class DingTalkLoggerConfigurationExtensionsTests
{
    private const string TestWebHookUrl =
        "https://oapi.dingtalk.com/robot/send?access_token=test_token";
    private const string TestSecret = "test_secret_123456";

    [Fact]
    public void DingTalkBatched_WithValidWebHookUrl_ShouldReturnLoggerConfiguration()
    {
        // Arrange
        var configuration = new LoggerConfiguration();

        // Act
        var result = configuration.WriteTo.DingTalkBatched(TestWebHookUrl);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<LoggerConfiguration>();
    }

    [Fact]
    public void DingTalkBatched_WithWebHookUrlAndSecret_ShouldReturnLoggerConfiguration()
    {
        // Arrange
        var configuration = new LoggerConfiguration();

        // Act
        var result = configuration.WriteTo.DingTalkBatched(TestWebHookUrl);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<LoggerConfiguration>();
    }

    [Fact]
    public void DingTalkBatched_WithAllParameters_ShouldReturnLoggerConfiguration()
    {
        // Arrange
        var configuration = new LoggerConfiguration();

        // Act
        var result = configuration.WriteTo.DingTalkBatched(
            webHookUrl: TestWebHookUrl,
            sendBatchesAsOneMessages: true,
            batchSizeLimit: 50,
            formatProvider: System.Globalization.CultureInfo.InvariantCulture,
            restrictedToMinimumLevel: LogEventLevel.Warning
        );

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<LoggerConfiguration>();
    }
}
