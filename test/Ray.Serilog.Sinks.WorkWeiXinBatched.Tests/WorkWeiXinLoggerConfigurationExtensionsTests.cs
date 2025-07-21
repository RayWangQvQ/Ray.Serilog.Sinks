using FluentAssertions;
using Serilog;
using Serilog.Events;

namespace Ray.Serilog.Sinks.WorkWeiXinBatched.Tests;

public class WorkWeiXinLoggerConfigurationExtensionsTests
{
    private const string TestWebHookUrl = "https://qyapi.weixin.qq.com/cgi-bin/webhook/send?key=test_key";

    [Fact]
    public void WorkWeiXinBatched_WithValidWebHookUrl_ShouldReturnLoggerConfiguration()
    {
        // Arrange
        var configuration = new LoggerConfiguration();

        // Act
        var result = configuration.WriteTo.WorkWeiXinBatched(TestWebHookUrl);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<LoggerConfiguration>();
    }

    [Fact]
    public void WorkWeiXinBatched_WithAllParameters_ShouldReturnLoggerConfiguration()
    {
        // Arrange
        var configuration = new LoggerConfiguration();

        // Act
        var result = configuration.WriteTo.WorkWeiXinBatched(
            webHookUrl: TestWebHookUrl,
            containsTrigger: "test",
            sendBatchesAsOneMessages: true,
            formatProvider: null,
            restrictedToMinimumLevel: LogEventLevel.Warning);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<LoggerConfiguration>();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void WorkWeiXinBatched_WithInvalidWebHookUrl_ShouldThrowArgumentException(string invalidWebHookUrl)
    {
        // Arrange
        var configuration = new LoggerConfiguration();

        // Act & Assert
        Action act = () => configuration.WriteTo.WorkWeiXinBatched(invalidWebHookUrl);
        act.Should().Throw<ArgumentException>();
    }
}
