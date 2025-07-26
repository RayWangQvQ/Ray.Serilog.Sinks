using FluentAssertions;
using Serilog;
using Serilog.Events;

namespace Ray.Serilog.Sinks.OtherApiBatched.Tests;

public class OtherApiLoggerConfigurationExtensionsTests
{
    private const string TestApiUrl = "https://api.example.com/webhook";

    [Fact]
    public void OtherApiBatched_WithValidApiUrl_ShouldReturnLoggerConfiguration()
    {
        // Arrange
        var configuration = new LoggerConfiguration();

        // Act
        var result = configuration.WriteTo.OtherApiBatched(
            TestApiUrl,
            "{\"message\": \"{{message}}\"}",
            "{{message}}"
        );

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<LoggerConfiguration>();
    }

    [Fact]
    public void OtherApiBatched_WithAllParameters_ShouldReturnLoggerConfiguration()
    {
        // Arrange
        var configuration = new LoggerConfiguration();

        // Act
        var result = configuration.WriteTo.OtherApiBatched(
            api: TestApiUrl,
            bodyJsonTemplate: "{\"message\": \"{{message}}\"}",
            placeholder: "{{message}}",
            sendBatchesAsOneMessages: true,
            batchSizeLimit: 50,
            formatProvider: System.Globalization.CultureInfo.InvariantCulture,
            restrictedToMinimumLevel: LogEventLevel.Warning
        );

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<LoggerConfiguration>();
    }
}
