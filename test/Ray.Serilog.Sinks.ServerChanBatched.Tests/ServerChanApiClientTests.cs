using FluentAssertions;

namespace Ray.Serilog.Sinks.ServerChanBatched.Tests;

public class ServerChanApiClientTests
{
    private const string TestSendKey = "test_sendkey_123456";

    [Fact]
    public void Constructor_WithValidSendKey_ShouldCreateInstance()
    {
        // Arrange & Act
        var client = new ServerChanApiClient(TestSendKey);

        // Assert
        client.Should().NotBeNull();
    }

    [Fact]
    public void PushMessage_WithValidParameters_ShouldReturnResponse()
    {
        // Arrange
        var client = new ServerChanApiClient(TestSendKey);

        // Act
        var act = () => client.PushMessage("Test Content", "Test Title");

        // Assert
        act.Should().NotThrow();
    }
}
