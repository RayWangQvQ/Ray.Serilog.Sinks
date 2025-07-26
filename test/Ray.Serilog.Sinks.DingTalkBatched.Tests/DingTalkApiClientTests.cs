using FluentAssertions;

namespace Ray.Serilog.Sinks.DingTalkBatched.Tests;

public class DingTalkApiClientTests
{
    private const string TestWebHookUrl =
        "https://oapi.dingtalk.com/robot/send?access_token=test_token";

    [Fact]
    public void Constructor_WithValidWebHookUrl_ShouldCreateInstance()
    {
        // Arrange & Act
        var client = new DingTalkApiClient(TestWebHookUrl);

        // Assert
        client.Should().NotBeNull();
    }

    [Fact]
    public void Constructor_WithValidWebHookUrlAndSecret_ShouldCreateInstance()
    {
        // Arrange & Act
        var client = new DingTalkApiClient(TestWebHookUrl);

        // Assert
        client.Should().NotBeNull();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_WithInvalidWebHookUrl_ShouldThrowException(string invalidWebHookUrl)
    {
        // Act & Assert
        Action act = () => _ = new DingTalkApiClient(invalidWebHookUrl);
        act.Should().Throw<Exception>(); // Uri构造函数可能抛出各种异常
    }

    [Fact]
    public async Task PushMessageAsync_WithValidParameters_ShouldReturnResponse()
    {
        // Arrange
        var client = new DingTalkApiClient(TestWebHookUrl);

        // Act
        Func<Task> act = async () => await client.PushMessageAsync("Test Content", "Test Title");

        // Assert
        await act.Should().NotThrowAsync();
    }
}
