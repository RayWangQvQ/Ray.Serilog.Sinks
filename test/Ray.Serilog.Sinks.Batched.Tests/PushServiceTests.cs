using FluentAssertions;

namespace Ray.Serilog.Sinks.Batched.Tests;

public class PushServiceTests
{
    [Fact]
    public void ClientName_ShouldReturnNonEmptyString()
    {
        // Arrange
        var pushService = new TestPushService();

        // Act
        var clientName = pushService.ClientName;

        // Assert
        clientName.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void PushMessage_WithValidParameters_ShouldComplete()
    {
        // Arrange
        var pushService = new TestPushService();

        // Act
        var result = pushService.PushMessage("Test Content", "Test Title");

        // Assert
        result.Should().NotBeNull();
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }

    [Theory]
    [InlineData("", "content")]
    [InlineData("title", "")]
    [InlineData(null, "content")]
    [InlineData("title", null)]
    public void PushMessage_WithInvalidParameters_ShouldHandleGracefully(
        string title,
        string content
    )
    {
        // Arrange
        var pushService = new TestPushService();

        // Act
        var act = () => pushService.PushMessage(content, title);

        // Assert
        act.Should().NotThrow();
    }

    private class TestPushService : PushService
    {
        public override string ClientName => "Test Push Service";

        public override HttpResponseMessage DoSend()
        {
            return new HttpResponseMessage(System.Net.HttpStatusCode.OK);
        }
    }
}
