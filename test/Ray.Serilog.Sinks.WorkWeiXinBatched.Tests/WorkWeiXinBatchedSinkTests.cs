using FluentAssertions;
using Serilog;
using Serilog.Events;

namespace Ray.Serilog.Sinks.WorkWeiXinBatched.Tests;

public class WorkWeiXinBatchedSinkTests
{
    private const string TestWebHookUrl =
        "https://qyapi.weixin.qq.com/cgi-bin/webhook/send?key=test_key";

    [Fact]
    public void Constructor_WithValidWebHookUrl_ShouldCreateInstance()
    {
        // Arrange & Act
        var sink = new WorkWeiXinBatchedSink(
            TestWebHookUrl,
            x => true,
            true,
            null,
            LogEventLevel.Information
        );

        // Assert
        sink.Should().NotBeNull();
    }
}
