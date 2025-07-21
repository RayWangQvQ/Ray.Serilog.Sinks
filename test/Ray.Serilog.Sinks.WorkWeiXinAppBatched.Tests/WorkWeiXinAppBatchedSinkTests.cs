using FluentAssertions;
using Serilog;
using Serilog.Events;
using Xunit;

namespace Ray.Serilog.Sinks.WorkWeiXinAppBatched.Tests;

public class WorkWeiXinAppBatchedSinkTests
{
    private const string TestCorpId = "test_corp_id_123456";
    private const string TestCorpSecret = "test_corp_secret_123456";
    private const string TestAgentId = "test_agent_id_123456";

    [Fact]
    public void Constructor_WithValidParameters_ShouldCreateInstance()
    {
        // Arrange & Act
        var sink = new WorkWeiXinAppBatchedSink(
            TestCorpId,
            TestCorpSecret,
            TestAgentId,
            "@all",
            "",
            "",
            x => true,
            true,
            "",
            null,
            LogEventLevel.Information
        );

        // Assert
        sink.Should().NotBeNull();
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
    public void Constructor_WithInvalidParameters_ShouldThrowArgumentException(
        string corpId,
        string corpSecret,
        string agentId
    )
    {
        // Act & Assert
        Action act = () =>
            new WorkWeiXinAppBatchedSink(
                corpId,
                corpSecret,
                agentId,
                "@all",
                "",
                "",
                x => true,
                true,
                "",
                null,
                LogEventLevel.Information
            );

        act.Should().Throw<ArgumentException>();
    }
}
