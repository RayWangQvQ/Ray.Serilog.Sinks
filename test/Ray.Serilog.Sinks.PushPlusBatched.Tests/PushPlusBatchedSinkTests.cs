using FluentAssertions;
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
            true,
            100,
            "{Message:lj}{NewLine}{Exception}",
            System.Globalization.CultureInfo.InvariantCulture,
            LogEventLevel.Information
        );

        // Assert
        sink.Should().NotBeNull();
    }
}
