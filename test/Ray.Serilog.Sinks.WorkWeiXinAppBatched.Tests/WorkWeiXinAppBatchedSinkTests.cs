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
}
