using FluentAssertions;
using Serilog;
using Serilog.Events;

namespace Ray.Serilog.Sinks.MicrosoftTeamsBatched.Tests;

public class MicrosoftTeamsBatchedSinkTests
{
    private const string TestWebHookUrl =
        "https://outlook.office.com/webhook/test/IncomingWebhook/test";

    [Fact]
    public void Constructor_WithValidWebHookUrl_ShouldCreateInstance()
    {
        // Arrange & Act
        var sink = new MicrosoftTeamsBatchedSink(
            TestWebHookUrl,
            true,
            100,
            "{Message}",
            System.Globalization.CultureInfo.InvariantCulture,
            LogEventLevel.Information
        );

        // Assert
        sink.Should().NotBeNull();
    }
}
