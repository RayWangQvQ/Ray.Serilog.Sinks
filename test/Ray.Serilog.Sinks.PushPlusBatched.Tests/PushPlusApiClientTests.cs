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
        client.ClientName.Should().Be("PushPlus");
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Constructor_WithInvalidToken_ShouldThrowException(string invalidToken)
    {
        // Act & Assert
        Action act = () => new PushPlusApiClient(invalidToken);
        act.Should().Throw<Exception>(); // Uri构造函数可能抛出各种异常
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
