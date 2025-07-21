using FluentAssertions;

namespace Ray.Serilog.Sinks.WorkWeiXinAppBatched.Tests;

public class WorkWeiXinAppApiClientTests
{
    private const string TestCorpId = "test_corp_id_123456";
    private const string TestCorpSecret = "test_corp_secret_123456";
    private const string TestAgentId = "test_agent_id_123456";

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
