using FluentAssertions;
using Serilog;
using Serilog.Events;

namespace Ray.Serilog.Sinks.MicrosoftTeamsBatched.Tests;

public class MicrosoftTeamsLoggerConfigurationExtensionsTests
{
    private const string TestWebHookUrl =
        "https://outlook.office.com/webhook/test/IncomingWebhook/test";

    [Fact]
    public void MicrosoftTeamsBatched_WithValidWebHookUrl_ShouldReturnLoggerConfiguration()
    {
        // Arrange
        var configuration = new LoggerConfiguration();

        // Act
        var result = configuration.WriteTo.MicrosoftTeamsBatched(TestWebHookUrl);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<LoggerConfiguration>();
    }

    [Fact]
    public void MicrosoftTeamsBatched_WithAllParameters_ShouldReturnLoggerConfiguration()
    {
        // Arrange
        var configuration = new LoggerConfiguration();

        // Act
        var result = configuration.WriteTo.MicrosoftTeamsBatched(
            webhook: TestWebHookUrl,
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
