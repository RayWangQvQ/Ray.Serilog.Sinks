using FluentAssertions;

namespace Ray.Serilog.Sinks.WorkWeiXinAppBatched.Tests;

public class WorkWeiXinAppApiClientTests
{
    private const string TestCorpId = "test_corp_id_123456";
    private const string TestCorpSecret = "test_corp_secret_123456";
    private const string TestAgentId = "test_agent_id_123456";

    [Fact]
    public void Constructor_WithValidParameters_ShouldCreateInstance()
    {
        // Arrange & Act
        var client = new WorkWeiXinAppApiClient(TestCorpId, TestCorpSecret, TestAgentId);

        // Assert
        client.Should().NotBeNull();
        client.ClientName.Should().Be("企业微信应用");
    }

    [Theory]
    [InlineData("", "secret", "agent")]
    [InlineData("   ", "secret", "agent")]
    [InlineData(null, "secret", "agent")]
    [InlineData("corp", "", "agent")]
    [InlineData("corp", "   ", "agent")]
    [InlineData("corp", null, "agent")]
    [InlineData("corp", "secret", "")]
    [InlineData("corp", "secret", "   ")]
    [InlineData("corp", "secret", null)]
    public void Constructor_WithInvalidParameters_ShouldThrowException(string corpId, string corpSecret, string agentId)
    {
        // Act & Assert
        Action act = () => new WorkWeiXinAppApiClient(corpId, corpSecret, agentId);
        act.Should().Throw<Exception>(); // 构造函数可能抛出各种异常
    }

    [Fact]
    public void PushMessage_WithValidParameters_ShouldReturnResponse()
    {
        // Arrange
        var client = new WorkWeiXinAppApiClient(TestCorpId, TestCorpSecret, TestAgentId);

        // Act
        var act = () => client.PushMessage("Test Content", "Test Title");

        // Assert
        act.Should().NotThrow();
    }
}
