using FluentAssertions;

namespace Ray.Serilog.Sinks.WorkWeiXinAppBatched.Tests;

public class WorkWeiXinAppApiClientTests
{
    private const string TestCorpId = "test_corp_id_123456";
    private const string TestCorpSecret = "test_corp_secret_123456";
    private const string TestAgentId = "test_agent_id_123456";

    [Fact]
    public async Task PushMessageAsync_WithValidParameters_ShouldReturnResponse()
    {
        // Arrange
        var client = new WorkWeiXinAppApiClient(TestCorpId, TestAgentId, TestCorpSecret);

        // Act
        Func<Task> act = async () => await client.PushMessageAsync("Test Content", "Test Title");

        // Assert
        await act.Should().NotThrowAsync();
    }
}
