using FluentAssertions;
using Serilog;
using Serilog.Events;

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
            x => true,
            true,
            null,
            LogEventLevel.Information);

        // Assert
        sink.Should().NotBeNull();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Constructor_WithInvalidSKey_ShouldThrowArgumentException(string invalidSKey)
    {
        // Act & Assert
        Action act = () => new CoolPushBatchedSink(
            invalidSKey,
            x => true,
            true,
            null,
            LogEventLevel.Information);

        act.Should().Throw<ArgumentException>();
    }
}
