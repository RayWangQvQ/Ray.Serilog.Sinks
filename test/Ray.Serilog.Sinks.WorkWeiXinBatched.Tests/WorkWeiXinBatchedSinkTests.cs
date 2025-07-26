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
            true,
            100,
            "{Message:lj}{NewLine}{Exception}",
            System.Globalization.CultureInfo.InvariantCulture,
            LogEventLevel.Information
        );

        // Assert
        sink.Should().NotBeNull();
    }
}
