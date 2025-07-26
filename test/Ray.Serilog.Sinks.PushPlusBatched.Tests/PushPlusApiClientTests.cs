using FluentAssertions;

namespace Ray.Serilog.Sinks.PushPlusBatched.Tests;

public class PushPlusApiClientTests
{
    private const string TestToken = "test_token_123456";

    [Fact]
    public void Constructor_WithValidToken_ShouldCreateInstance()
    {
        // Arrange & Act
        var client = new PushPlusApiClient(TestToken);

        // Assert
        client.Should().NotBeNull();
    }

    [Fact]
    public async Task PushMessageAsync_WithValidParameters_ShouldReturnResponse()
    {
        // Arrange
        var client = new PushPlusApiClient(TestToken);

        // Act
        Func<Task> act = async () => await client.PushMessageAsync("Test Content", "Test Title");

        // Assert
        await act.Should().NotThrowAsync();
    }
}
