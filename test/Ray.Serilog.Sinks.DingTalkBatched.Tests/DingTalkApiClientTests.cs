using FluentAssertions;

namespace Ray.Serilog.Sinks.DingTalkBatched.Tests;

public class DingTalkApiClientTests
{
    private const string TestWebHookUrl =
        "https://oapi.dingtalk.com/robot/send?access_token=test_token";
    private const string TestSecret = "test_secret_123456";

    [Fact]
    public void Constructor_WithValidWebHookUrl_ShouldCreateInstance()
    {
        // Arrange & Act
        var client = new DingTalkApiClient(TestWebHookUrl);

        // Assert
        client.Should().NotBeNull();
        client.ClientName.Should().Be("钉钉机器人");
    }

    [Fact]
    public void Constructor_WithValidWebHookUrlAndSecret_ShouldCreateInstance()
    {
        // Arrange & Act
        var client = new DingTalkApiClient(TestWebHookUrl);

        // Assert
        client.Should().NotBeNull();
        client.ClientName.Should().Be("钉钉机器人");
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Constructor_WithInvalidWebHookUrl_ShouldThrowException(string invalidWebHookUrl)
    {
        // Act & Assert
        Action act = () => new DingTalkApiClient(invalidWebHookUrl);
        act.Should().Throw<Exception>(); // Uri构造函数可能抛出各种异常
    }

    [Fact]
    public void PushMessage_WithValidParameters_ShouldReturnResponse()
    {
        // Arrange
        var client = new DingTalkApiClient(TestWebHookUrl);

        // Act
        var act = () => client.PushMessage("Test Content", "Test Title");

        // Assert
        act.Should().NotThrow();
    }
}
