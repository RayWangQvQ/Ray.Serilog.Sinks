using FluentAssertions;

namespace Ray.Serilog.Sinks.Batched.Tests;

public class PushServiceTests
{
    [Fact]
    public async Task PushMessage_WithValidParameters_ShouldComplete()
    {
        // Arrange
        var pushService = new TestPushService();

        // Act
        var result = await pushService.PushMessageAsync("Test Content", "Test Title");

        // Assert
        result.Should().NotBeNull();
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }

    [Theory]
    [InlineData("", "content")]
    [InlineData("title", "")]
    [InlineData("title", "content")]
    public async Task PushMessage_WithInvalidParameters_ShouldHandleGracefully(
        string? title,
        string content
    )
    {
        // Arrange
        var pushService = new TestPushService();

        // Act
        var act = async () => await pushService.PushMessageAsync(content, title ?? "");

        // Assert
        await act.Should().NotThrowAsync();
    }

    private class TestPushService : PushService
    {
        protected override string ClientName => "Test Push Service";

        protected override Task<HttpResponseMessage> DoSendAsync(string message, string title = "")
        {
            return Task.FromResult(new HttpResponseMessage(System.Net.HttpStatusCode.OK));
        }
    }
}
