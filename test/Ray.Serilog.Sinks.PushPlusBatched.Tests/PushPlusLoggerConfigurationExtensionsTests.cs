using FluentAssertions;
using Serilog;
using Serilog.Events;

namespace Ray.Serilog.Sinks.PushPlusBatched.Tests;

public class PushPlusLoggerConfigurationExtensionsTests
{
    private const string TestToken = "test_token_123456";

    [Fact]
    public void PushPlusBatched_WithValidToken_ShouldReturnLoggerConfiguration()
    {
        // Arrange
        var configuration = new LoggerConfiguration();

        // Act
        var result = configuration.WriteTo.PushPlusBatched(TestToken);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<LoggerConfiguration>();
    }

    [Fact]
    public void PushPlusBatched_WithAllParameters_ShouldReturnLoggerConfiguration()
    {
        // Arrange
        var configuration = new LoggerConfiguration();

        // Act
        var result = configuration.WriteTo.PushPlusBatched(
            token: TestToken,
            topic: "test",
            channel: "wechat",
            webhook: "",
            containsTrigger: "test",
            sendBatchesAsOneMessages: true,
            outputTemplate: "{Message}",
            formatProvider: null,
            restrictedToMinimumLevel: LogEventLevel.Warning
        );

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<LoggerConfiguration>();
    }
}
