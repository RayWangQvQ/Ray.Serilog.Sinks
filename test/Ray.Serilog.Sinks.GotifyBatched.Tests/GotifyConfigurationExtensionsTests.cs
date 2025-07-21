using FluentAssertions;
using Serilog;
using Serilog.Events;
using Xunit;

namespace Ray.Serilog.Sinks.GotifyBatched.Tests;

public class GotifyConfigurationExtensionsTests
{
    private const string TestHost = "https://gotify.example.com";
    private const string TestToken = "test_app_token_123456";

    [Fact]
    public void GotifyBatched_WithValidParameters_ShouldReturnLoggerConfiguration()
    {
        // Arrange
        var configuration = new LoggerConfiguration();

        // Act
        var result = configuration.WriteTo.GotifyBatched(TestHost, TestToken);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<LoggerConfiguration>();
    }

    [Fact]
    public void GotifyBatched_WithAllParameters_ShouldReturnLoggerConfiguration()
    {
        // Arrange
        var configuration = new LoggerConfiguration();

        // Act
        var result = configuration.WriteTo.GotifyBatched(
            host: TestHost,
            token: TestToken,
            restrictedToMinimumLevel: LogEventLevel.Warning,
            containsTrigger: "test",
            sendBatchesAsOneMessages: true,
            outputTemplate: "{Message}",
            formatProvider: null);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<LoggerConfiguration>();
    }

    [Theory]
    [InlineData("", "token")]
    [InlineData("   ", "token")]
    [InlineData(null, "token")]
    [InlineData("https://example.com", "")]
    [InlineData("https://example.com", "   ")]
    [InlineData("https://example.com", null)]
    public void GotifyBatched_WithInvalidParameters_ShouldThrowArgumentException(string host, string token)
    {
        // Arrange
        var configuration = new LoggerConfiguration();

        // Act & Assert
        Action act = () => configuration.WriteTo.GotifyBatched(host, token);
        act.Should().Throw<Exception>();
    }
}
