using FluentAssertions;
using Serilog;
using Serilog.Events;
using Xunit;

namespace Ray.Serilog.Sinks.DingTalkBatched.Tests;

public class DingTalkBatchedSinkTests
{
    private const string TestWebHookUrl =
        "https://oapi.dingtalk.com/robot/send?access_token=test_token";
    private const string TestSecret = "test_secret_123456";

    [Fact]
    public void Constructor_WithValidWebHookUrl_ShouldCreateInstance()
    {
        // Arrange & Act
        var sink = new DingTalkBatchedSink(
            TestWebHookUrl,
            true,
            100,
            System.Globalization.CultureInfo.InvariantCulture,
            LogEventLevel.Information
        );

        // Assert
        sink.Should().NotBeNull();
    }

    [Fact]
    public void Constructor_WithValidWebHookUrlAndSecret_ShouldCreateInstance()
    {
        // Arrange & Act
        var sink = new DingTalkBatchedSink(
            TestWebHookUrl,
            true,
            50,
            System.Globalization.CultureInfo.InvariantCulture,
            LogEventLevel.Information
        );

        // Assert
        sink.Should().NotBeNull();
    }
}
