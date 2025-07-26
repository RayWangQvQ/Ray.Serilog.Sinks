using FluentAssertions;

namespace Ray.Serilog.Sinks.TelegramBatched.Tests;

public class TelegramApiClientTests
{
    private const string TestBotToken = "123456789:ABCDEFGHIJKLMNOP-qrstuvwxyz";
    private const string TestChatId = "-1001234567890";

    [Fact]
    public void Constructor_WithValidBotToken_ShouldCreateInstance()
    {
        // Arrange & Act
        var client = new TelegramApiClient(TestBotToken, TestChatId);

        // Assert
        client.Should().NotBeNull();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Constructor_WithInvalidBotToken_ShouldThrowArgumentException(string invalidBotToken)
    {
        // Act & Assert
        Action act = () => new TelegramApiClient(invalidBotToken, TestChatId);
        act.Should().Throw<ArgumentException>().WithMessage("The bot token mustn't be empty.*");
    }

    [Fact]
    public void Constructor_WithProxyString_ShouldCreateInstance()
    {
        // Arrange & Act
        var client = new TelegramApiClient(
            TestBotToken,
            TestChatId,
            "http://proxy.example.com:8080"
        );

        // Assert
        client.Should().NotBeNull();
    }

    [Fact]
    public void Constructor_WithTimeoutSeconds_ShouldCreateInstance()
    {
        // Arrange & Act
        var client = new TelegramApiClient(TestBotToken, TestChatId, "", 30);

        // Assert
        client.Should().NotBeNull();
    }
}
