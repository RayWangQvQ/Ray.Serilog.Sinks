using FluentAssertions;
using Serilog;
using Serilog.Events;
using Xunit;

namespace Ray.Serilog.Sinks.GotifyBatched.Tests;

public class GotifyConfigurationExtensionsTests
{
    private const string TestHost = "https://gotify.example.com";
    private const string TestToken = "test_app_token_123456";

    [Fact]
    public void GotifyBatched_WithValidParameters_ShouldReturnLoggerConfiguration()
    {
        // Arrange
        var configuration = new LoggerConfiguration();

        // Act
        var result = configuration.WriteTo.GotifyBatched(TestHost, TestToken);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<LoggerConfiguration>();
    }

    [Fact]
    public void GotifyBatched_WithAllParameters_ShouldReturnLoggerConfiguration()
    {
        // Arrange
        var configuration = new LoggerConfiguration();

        // Act
        var result = configuration.WriteTo.GotifyBatched(
            host: TestHost,
            token: TestToken,
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
