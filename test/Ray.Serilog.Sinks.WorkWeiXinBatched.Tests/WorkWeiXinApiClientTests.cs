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
        client.ClientName.Should().Be("企业微信机器人");
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Constructor_WithInvalidWebHookUrl_ShouldThrowException(string invalidWebHookUrl)
    {
        // Act & Assert
        Action act = () => new WorkWeiXinApiClient(invalidWebHookUrl);
        act.Should().Throw<Exception>(); // Uri构造函数可能抛出各种异常
    }

    [Fact]
    public void PushMessage_WithValidParameters_ShouldReturnResponse()
    {
        // Arrange
        var client = new WorkWeiXinApiClient(TestWebHookUrl);

        // Act
        var act = () => client.PushMessage("Test Content", "Test Title");

        // Assert
        act.Should().NotThrow();
    }
}
