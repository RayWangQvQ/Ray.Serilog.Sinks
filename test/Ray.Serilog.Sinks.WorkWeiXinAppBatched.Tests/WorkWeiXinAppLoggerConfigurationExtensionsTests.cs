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
        var result = configuration.WriteTo.WorkWeiXinAppBatched(
            TestCorpId,
            TestAgentId,
            TestCorpSecret,
            "@all",
            "",
            ""
        );

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
            sendBatchesAsOneMessages: true,
            batchSizeLimit: 50,
            outputTemplate: "{Message}",
            formatProvider: System.Globalization.CultureInfo.InvariantCulture,
            restrictedToMinimumLevel: LogEventLevel.Warning
        );

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<LoggerConfiguration>();
    }
}
