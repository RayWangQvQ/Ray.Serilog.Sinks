using FluentAssertions;
using Serilog;
using Serilog.Events;

namespace Ray.Serilog.Sinks.OtherApiBatched.Tests;

public class OtherApiBatchedSinkTests
{
    private const string TestApiUrl = "https://api.example.com/webhook";

    [Fact]
    public void Constructor_WithValidApiUrl_ShouldCreateInstance()
    {
        // Arrange & Act
        var sink = new OtherApiBatchedSink(
            TestApiUrl,
            "{\"message\": \"{{message}}\"}",
            "{{message}}",
            x => true,
            true,
            null,
            LogEventLevel.Information
        );

        // Assert
        sink.Should().NotBeNull();
    }
}
