using FluentAssertions;
using Ray.Serilog.Sinks.MicrosoftTeamsBatched;
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
            x => true,
            true,
            "{Message}",
            null,
            LogEventLevel.Information);

        // Assert
        sink.Should().NotBeNull();
    }

    [Theory]
    [InlineData("", "token")]
    [InlineData("   ", "token")]
    [InlineData(null, "token")]
    [InlineData("https://example.com", "")]
    [InlineData("https://example.com", "   ")]
    [InlineData("https://example.com", null)]
    public void Constructor_WithInvalidParameters_ShouldThrowArgumentException(string host, string token)
    {
        // Act & Assert
        Action act = () => new GotifyBatchedSink(
            host,
            token,
            x => true,
            true,
            "{Message}",
            null,
            LogEventLevel.Information);

        act.Should().Throw<ArgumentException>();
    }
}
