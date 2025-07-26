using FluentAssertions;
using Serilog;
using Serilog.Events;
using Xunit;

namespace Ray.Serilog.Sinks.GotifyBatched.Tests;

public class GotifyBatchedSinkTests
{
    private const string TestHost = "https://gotify.example.com";
    private const string TestToken = "test_app_token_123456";

    [Fact]
    public void Constructor_WithValidParameters_ShouldCreateInstance()
    {
        // Arrange & Act
        var sink = new GotifyBatchedSink(
            TestHost,
            TestToken,
            true,
            100,
            "{Message}",
            System.Globalization.CultureInfo.InvariantCulture,
            LogEventLevel.Information
        );

        // Assert
        sink.Should().NotBeNull();
    }
}
