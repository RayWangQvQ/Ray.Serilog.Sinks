using FluentAssertions;
using Serilog;
using Serilog.Events;

namespace Ray.Serilog.Sinks.PushPlusBatched.Tests;

public class PushPlusBatchedSinkTests
{
    private const string TestToken = "test_token_123456";

    [Fact]
    public void Constructor_WithValidToken_ShouldCreateInstance()
    {
        // Arrange & Act
        var sink = new PushPlusBatchedSink(
            TestToken,
            "",
            "",
            "",
            x => true,
            true,
            "",
            null,
            LogEventLevel.Information);

        // Assert
        sink.Should().NotBeNull();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Constructor_WithInvalidToken_ShouldThrowArgumentException(string invalidToken)
    {
        // Act & Assert
        Action act = () => new PushPlusBatchedSink(
            invalidToken,
            "",
            "",
            "",
            x => true,
            true,
            "",
            null,
            LogEventLevel.Information);

        act.Should().Throw<ArgumentException>();
    }
}
