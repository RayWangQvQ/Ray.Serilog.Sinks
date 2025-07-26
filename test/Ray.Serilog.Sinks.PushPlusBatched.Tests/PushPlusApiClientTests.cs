using FluentAssertions;

namespace Ray.Serilog.Sinks.PushPlusBatched.Tests;

public class PushPlusApiClientTests
{
    private const string TestToken = "test_token_123456";

    [Fact]
    public void Constructor_WithValidToken_ShouldCreateInstance()
    {
        // Arrange & Act
        var client = new PushPlusApiClient(TestToken);

        // Assert
        client.Should().NotBeNull();
    }

    [Fact]
    public void PushMessage_WithValidParameters_ShouldReturnResponse()
    {
        // Arrange
        var client = new PushPlusApiClient(TestToken);

        // Act
        var act = () => client.PushMessage("Test Content", "Test Title");

        // Assert
        act.Should().NotThrow();
    }
}
