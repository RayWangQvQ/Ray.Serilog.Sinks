using FluentAssertions;

namespace Ray.Serilog.Sinks.CoolPushBatched.Tests;

public class CoolPushApiClientTests
{
    private const string TestSKey = "test_skey_123456";

    [Fact]
    public void Constructor_WithValidSKey_ShouldCreateInstance()
    {
        // Arrange & Act
        var client = new CoolPushApiClient(TestSKey);

        // Assert
        client.Should().NotBeNull();
        client.ClientName.Should().Be("酷推");
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Constructor_WithInvalidSKey_ShouldThrowException(string invalidSKey)
    {
        // Act & Assert
        Action act = () => new CoolPushApiClient(invalidSKey);
        act.Should().Throw<Exception>(); // Uri构造函数可能抛出各种异常
    }

    [Fact]
    public void PushMessage_WithValidParameters_ShouldReturnResponse()
    {
        // Arrange
        var client = new CoolPushApiClient(TestSKey);

        // Act
        var act = () => client.PushMessage("Test Content", "Test Title");

        // Assert
        act.Should().NotThrow();
    }
}
