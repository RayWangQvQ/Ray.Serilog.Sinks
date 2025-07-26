using FluentAssertions;

namespace Ray.Serilog.Sinks.MicrosoftTeamsBatched.Tests;

public class MicrosoftTeamsApiClientTests
{
    private const string TestWebHookUrl =
        "https://outlook.office.com/webhook/test/IncomingWebhook/test";

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_WithInvalidWebHookUrl_ShouldThrowException(string invalidWebHookUrl)
    {
        // Act & Assert
        Action act = () => _ = new MicrosoftTeamsApiClient(invalidWebHookUrl);
        act.Should().Throw<Exception>(); // Uri构造函数可能抛出各种异常
    }

    [Fact]
    public async Task PushMessageAsync_WithValidParameters_ShouldReturnResponse()
    {
        // Arrange
        var client = new MicrosoftTeamsApiClient(TestWebHookUrl);

        // Act
        Func<Task> act = async () => await client.PushMessageAsync("Test Content", "Test Title");

        // Assert
        await act.Should().NotThrowAsync();
    }
}
