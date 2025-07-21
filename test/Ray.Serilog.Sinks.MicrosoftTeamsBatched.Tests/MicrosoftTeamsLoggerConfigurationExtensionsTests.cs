using FluentAssertions;
using Serilog;
using Serilog.Events;

namespace Ray.Serilog.Sinks.MicrosoftTeamsBatched.Tests;

public class MicrosoftTeamsLoggerConfigurationExtensionsTests
{
    private const string TestWebHookUrl = "https://outlook.office.com/webhook/test/IncomingWebhook/test";

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
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void MicrosoftTeamsBatched_WithInvalidWebHookUrl_ShouldThrowArgumentException(string invalidWebHookUrl)
    {
        // Arrange
        var configuration = new LoggerConfiguration();

        // Act & Assert
        Action act = () => configuration.WriteTo.MicrosoftTeamsBatched(invalidWebHookUrl);
        act.Should().Throw<ArgumentException>();
    }
}
