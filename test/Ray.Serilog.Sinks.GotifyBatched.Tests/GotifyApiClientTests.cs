using FluentAssertions;
using Ray.Serilog.Sinks.MicrosoftTeamsBatched;

namespace Ray.Serilog.Sinks.GotifyBatched.Tests;

public class GotifyApiClientTests
{
    private const string TestHost = "https://gotify.example.com";
    private const string TestToken = "test_app_token_123456";

    [Fact]
    public void Constructor_WithValidParameters_ShouldCreateInstance()
    {
        // Arrange & Act
        var client = new GotifyApiClient(TestHost, TestToken);

        // Assert
        client.Should().NotBeNull();
        client.ClientName.Should().Be("Gotify");
    }

    [Theory]
    [InlineData("", "token")]
    [InlineData("   ", "token")]
    [InlineData(null, "token")]
    [InlineData("https://example.com", "")]
    [InlineData("https://example.com", "   ")]
    [InlineData("https://example.com", null)]
    public void Constructor_WithInvalidParameters_ShouldThrowException(string host, string token)
    {
        // Act & Assert
        Action act = () => new GotifyApiClient(host, token);
        act.Should().Throw<Exception>();
    }

    [Fact]
    public void PushMessage_WithValidParameters_ShouldReturnResponse()
    {
        // Arrange
        var client = new GotifyApiClient(TestHost, TestToken);

        // Act
        var act = () => client.PushMessage("Test Content", "Test Title");

        // Assert
        act.Should().NotThrow();
    }
}
