using FluentAssertions;

namespace Ray.Serilog.Sinks.OtherApiBatched.Tests;

public class OtherApiClientTests
{
    private const string TestApiUrl = "https://api.example.com/webhook";

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Constructor_WithInvalidApiUrl_ShouldThrowException(string invalidApiUrl)
    {
        // Act & Assert
        Action act = () => new OtherApiClient(invalidApiUrl, "{}", "test");
        act.Should().Throw<Exception>();
    }
}
