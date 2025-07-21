using FluentAssertions;
using Serilog;
using Serilog.Events;

namespace Ray.Serilog.Sinks.ServerChanBatched.Tests;

public class ServerChanBatchedSinkTests
{
    private const string TestSendKey = "test_sendkey_123456";

    [Fact]
    public void Constructor_WithValidSendKey_ShouldCreateInstance()
    {
        // Arrange & Act
        var sink = new ServerChanBatchedSink(
            TestSendKey,
            "",
            x => true,
            true,
            null,
            LogEventLevel.Information
        );

        // Assert
        sink.Should().NotBeNull();
    }
}
