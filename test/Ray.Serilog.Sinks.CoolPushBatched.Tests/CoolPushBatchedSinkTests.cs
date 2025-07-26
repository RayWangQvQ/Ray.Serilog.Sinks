using FluentAssertions;
using Serilog;
using Serilog.Events;
using Xunit;

namespace Ray.Serilog.Sinks.CoolPushBatched.Tests;

public class CoolPushBatchedSinkTests
{
    private const string TestSKey = "test_skey_123456";

    [Fact]
    public void Constructor_WithValidParameters_ShouldCreateInstance()
    {
        // Arrange & Act
        var sink = new CoolPushBatchedSink(
            TestSKey,
            true,
            100,
            System.Globalization.CultureInfo.InvariantCulture,
            LogEventLevel.Information
        );

        // Assert
        sink.Should().NotBeNull();
    }
}
