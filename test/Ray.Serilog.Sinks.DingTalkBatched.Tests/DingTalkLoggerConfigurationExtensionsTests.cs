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
        var result = configuration.WriteTo.DingTalkBatched(TestWebHookUrl, TestSecret);

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
            containsTrigger: "test",
            sendBatchesAsOneMessages: true,
            formatProvider: null,
            restrictedToMinimumLevel: LogEventLevel.Warning
        );

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<LoggerConfiguration>();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void DingTalkBatched_WithInvalidWebHookUrl_ShouldThrowArgumentException(
        string invalidWebHookUrl
    )
    {
        // Arrange
        var configuration = new LoggerConfiguration();

        // Act & Assert
        Action act = () => configuration.WriteTo.DingTalkBatched(invalidWebHookUrl);
        act.Should().Throw<ArgumentException>();
    }
}
