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
            x => true,
            true,
            "{Message}",
            null,
            LogEventLevel.Information
        );

        // Assert
        sink.Should().NotBeNull();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Constructor_WithInvalidWebHookUrl_ShouldThrowArgumentException(
        string invalidWebHookUrl
    )
    {
        // Act & Assert
        Action act = () =>
            new MicrosoftTeamsBatchedSink(
                invalidWebHookUrl,
                x => true,
                true,
                "{Message}",
                null,
                LogEventLevel.Information
            );

        act.Should().Throw<ArgumentException>();
    }
}
