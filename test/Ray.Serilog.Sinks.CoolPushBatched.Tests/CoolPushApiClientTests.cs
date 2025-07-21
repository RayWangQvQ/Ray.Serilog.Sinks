using FluentAssertions;

namespace Ray.Serilog.Sinks.CoolPushBatched.Tests;

public class CoolPushApiClientTests
{
    private const string TestSKey = "test_skey_123456";

    [Fact]
    public void Constructor_WithValidSKey_ShouldCreateInstance()
    {
        // Arrange & Act
        var client = new CoolPushApiClient(TestSKey);

        // Assert
        client.Should().NotBeNull();
        client.ClientName.Should().Be("酷推");
    }
}
