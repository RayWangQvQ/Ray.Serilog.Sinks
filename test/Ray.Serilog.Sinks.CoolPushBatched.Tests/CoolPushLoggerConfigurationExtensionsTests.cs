using FluentAssertions;
using Serilog;
using Serilog.Events;
using Xunit;

namespace Ray.Serilog.Sinks.CoolPushBatched.Tests;

public class CoolPushLoggerConfigurationExtensionsTests
{
    private const string TestSKey = "test_skey_123456";

    [Fact]
    public void CoolPushBatched_WithValidConfiguration_ShouldReturnLoggerConfiguration()
    {
        // Arrange
        var configuration = new LoggerConfiguration();

        // Act
        var result = configuration.WriteTo.CoolPushBatched(TestSKey);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<LoggerConfiguration>();
    }

    [Fact]
    public void CoolPushBatched_WithAllParameters_ShouldReturnLoggerConfiguration()
    {
        // Arrange
        var configuration = new LoggerConfiguration();

        // Act
        var result = configuration.WriteTo.CoolPushBatched(
            sKey: TestSKey,
            restrictedToMinimumLevel: LogEventLevel.Warning,
            containsTrigger: "test",
            sendBatchesAsOneMessages: true,
            formatProvider: null
        );

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<LoggerConfiguration>();
    }
}
