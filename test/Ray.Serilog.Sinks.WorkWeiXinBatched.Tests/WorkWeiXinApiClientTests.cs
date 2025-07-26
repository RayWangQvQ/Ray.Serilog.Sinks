using FluentAssertions;

namespace Ray.Serilog.Sinks.WorkWeiXinBatched.Tests;

public class WorkWeiXinApiClientTests
{
    private const string TestWebHookUrl =
        "https://qyapi.weixin.qq.com/cgi-bin/webhook/send?key=test_key";

    [Fact]
    public void Constructor_WithValidWebHookUrl_ShouldCreateInstance()
    {
        // Arrange & Act
        var client = new WorkWeiXinApiClient(TestWebHookUrl);

        // Assert
        client.Should().NotBeNull();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_WithInvalidWebHookUrl_ShouldThrowException(string invalidWebHookUrl)
    {
        // Act & Assert
        Action act = () => _ = new WorkWeiXinApiClient(invalidWebHookUrl);
        act.Should().Throw<Exception>(); // Uri构造函数可能抛出各种异常
    }

    [Fact]
    public async Task PushMessageAsync_WithValidParameters_ShouldReturnResponse()
    {
        // Arrange
        var client = new WorkWeiXinApiClient(TestWebHookUrl);

        // Act
        Func<Task> act = async () => await client.PushMessageAsync("Test Content", "Test Title");

        // Assert
        await act.Should().NotThrowAsync();
    }
}
