using FluentAssertions;
using Serilog;
using Serilog.Events;

namespace Ray.Serilog.Sinks.ServerChanBatched.Tests;

public class ServerChanLoggerConfigurationExtensionsTests
{
    private const string TestSendKey = "test_sendkey_123456";

    [Fact]
    public void ServerChanBatched_WithValidSendKey_ShouldReturnLoggerConfiguration()
    {
        // Arrange
        var configuration = new LoggerConfiguration();

        // Act
        var result = configuration.WriteTo.ServerChanBatched(TestSendKey, "");

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<LoggerConfiguration>();
    }

    [Fact]
    public void ServerChanBatched_WithAllParameters_ShouldReturnLoggerConfiguration()
    {
        // Arrange
        var configuration = new LoggerConfiguration();

        // Act
        var result = configuration.WriteTo.ServerChanBatched(
            scKey: TestSendKey,
            turboScKey: "",
            containsTrigger: "test",
            sendBatchesAsOneMessages: true,
            formatProvider: null,
            restrictedToMinimumLevel: LogEventLevel.Warning
        );

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<LoggerConfiguration>();
    }
}
