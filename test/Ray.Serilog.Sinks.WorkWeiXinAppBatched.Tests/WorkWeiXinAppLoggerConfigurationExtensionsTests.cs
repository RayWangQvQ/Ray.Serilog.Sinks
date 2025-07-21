using FluentAssertions;
using Serilog;
using Serilog.Events;

namespace Ray.Serilog.Sinks.WorkWeiXinAppBatched.Tests;

public class WorkWeiXinAppLoggerConfigurationExtensionsTests
{
    private const string TestCorpId = "test_corp_id_123456";
    private const string TestCorpSecret = "test_corp_secret_123456";
    private const string TestAgentId = "test_agent_id_123456";

    [Fact]
    public void WorkWeiXinAppBatched_WithValidParameters_ShouldReturnLoggerConfiguration()
    {
        // Arrange
        var configuration = new LoggerConfiguration();

        // Act
        var result = configuration.WriteTo.WorkWeiXinAppBatched(TestCorpId, TestAgentId, TestCorpSecret, "@all", "", "");

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<LoggerConfiguration>();
    }

    [Fact]
    public void WorkWeiXinAppBatched_WithAllParameters_ShouldReturnLoggerConfiguration()
    {
        // Arrange
        var configuration = new LoggerConfiguration();

        // Act
        var result = configuration.WriteTo.WorkWeiXinAppBatched(
            corpId: TestCorpId,
            agentId: TestAgentId,
            secret: TestCorpSecret,
            toUser: "@all",
            toParty: "",
            toTag: "",
            restrictedToMinimumLevel: LogEventLevel.Warning,
            containsTrigger: "test",
            sendBatchesAsOneMessages: true,
            outputTemplate: "{Message}",
            formatProvider: null);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<LoggerConfiguration>();
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
    public void WorkWeiXinAppBatched_WithInvalidParameters_ShouldThrowArgumentException(string corpId, string corpSecret, string agentId)
    {
        // Arrange
        var configuration = new LoggerConfiguration();

        // Act & Assert
        Action act = () => configuration.WriteTo.WorkWeiXinAppBatched(corpId, agentId, corpSecret, "@all", "", "");
        act.Should().Throw<ArgumentException>();
    }
}
