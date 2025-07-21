using FluentAssertions;

namespace Ray.Serilog.Sinks.OtherApiBatched.Tests;

public class OtherApiClientTests
{
    private const string TestApiUrl = "https://api.example.com/webhook";

    [Fact]
    public void Constructor_WithValidApiUrl_ShouldCreateInstance()
    {
        // Arrange & Act
        var client = new OtherApiClient(TestApiUrl, "{\"message\": \"{{message}}\"}", "{{message}}");

        // Assert
        client.Should().NotBeNull();
        client.ClientName.Should().Be("第三方API");
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Constructor_WithInvalidApiUrl_ShouldThrowException(string invalidApiUrl)
    {
        // Act & Assert
        Action act = () => new OtherApiClient(invalidApiUrl, "{}", "test");
        act.Should().Throw<Exception>();
    }

    [Fact]
    public void PushMessage_WithValidParameters_ShouldReturnResponse()
    {
        // Arrange
        var client = new OtherApiClient(TestApiUrl, "{\"message\": \"{{message}}\"}", "{{message}}");

        // Act
        var act = () => client.PushMessage("Test Content", "Test Title");

        // Assert
        act.Should().NotThrow();
    }
}
