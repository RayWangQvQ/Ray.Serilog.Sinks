using FluentAssertions;

namespace Ray.Serilog.Sinks.Batched.Tests;

public class PushServiceTests
{
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
        protected override string ClientName => "Test Push Service";

        protected override HttpResponseMessage DoSend()
        {
            return new HttpResponseMessage(System.Net.HttpStatusCode.OK);
        }
    }
}
