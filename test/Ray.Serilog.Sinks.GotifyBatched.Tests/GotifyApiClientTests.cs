using FluentAssertions;

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
}
