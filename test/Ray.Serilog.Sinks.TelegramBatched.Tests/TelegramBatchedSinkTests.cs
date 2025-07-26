using FluentAssertions;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace Ray.Serilog.Sinks.TelegramBatched.Tests;

public class TelegramBatchedSinkTests
{
    private const string TestBotToken = "123456789:ABCDEFGHIJKLMNOP-qrstuvwxyz";
    private const string TestChatId = "-1001234567890";
    private const string TestProxy = "http://proxy.example.com:8080";

    [Fact]
    public void Constructor_WithValidParameters_ShouldCreateInstance()
    {
        // Arrange & Act
        var sink = new TelegramBatchedSink(
            TestBotToken,
            TestChatId,
            TestProxy,
            true,
            100,
            "{Message:lj}{NewLine}{Exception}",
            System.Globalization.CultureInfo.InvariantCulture,
            LogEventLevel.Information
        );

        // Assert
        sink.Should().NotBeNull();
    }

    [Fact]
    public void Constructor_WithEmptyBotToken_ShouldStillCreateInstance()
    {
        // Arrange & Act
        var sink = new TelegramBatchedSink(
            "",
            TestChatId,
            TestProxy,
            true,
            100,
            "{Message:lj}{NewLine}{Exception}",
            System.Globalization.CultureInfo.InvariantCulture,
            LogEventLevel.Information
        );

        // Assert
        sink.Should().NotBeNull();
    }

    [Fact]
    public void Emit_WithEmptyBotToken_ShouldNotProcessLogEvent()
    {
        // Arrange
        var sink = new TelegramBatchedSink(
            "",
            TestChatId,
            TestProxy,
            true,
            100,
            "{Message:lj}{NewLine}{Exception}",
            System.Globalization.CultureInfo.InvariantCulture,
            LogEventLevel.Information
        );

        var logEvent = CreateTestLogEvent(LogEventLevel.Information, "Test message");

        // Act & Assert (should not throw)
        sink.Invoking(s => s.Emit(logEvent)).Should().NotThrow();
    }

    [Fact]
    public void Emit_WithEmptyChatId_ShouldNotProcessLogEvent()
    {
        // Arrange
        var sink = new TelegramBatchedSink(
            TestBotToken,
            "",
            TestProxy,
            true,
            100,
            "{Message:lj}{NewLine}{Exception}",
            System.Globalization.CultureInfo.InvariantCulture,
            LogEventLevel.Information
        );

        var logEvent = CreateTestLogEvent(LogEventLevel.Information, "Test message");

        // Act & Assert (should not throw)
        sink.Invoking(s => s.Emit(logEvent)).Should().NotThrow();
    }

    [Fact]
    public void Emit_WithNullLogEvent_ShouldThrowArgumentNullException()
    {
        // Arrange
        var sink = new TelegramBatchedSink(
            TestBotToken,
            TestChatId,
            TestProxy,
            true,
            100,
            "{Message:lj}{NewLine}{Exception}",
            System.Globalization.CultureInfo.InvariantCulture,
            LogEventLevel.Information
        );

        // Act & Assert
        sink.Invoking(s => s.Emit(null!)).Should().Throw<ArgumentNullException>();
    }

    [Theory]
    [InlineData(LogEventLevel.Verbose)]
    [InlineData(LogEventLevel.Debug)]
    [InlineData(LogEventLevel.Information)]
    [InlineData(LogEventLevel.Warning)]
    [InlineData(LogEventLevel.Error)]
    [InlineData(LogEventLevel.Fatal)]
    public void Constructor_WithDifferentLogLevels_ShouldCreateInstance(LogEventLevel logLevel)
    {
        // Arrange & Act
        var sink = new TelegramBatchedSink(
            TestBotToken,
            TestChatId,
            TestProxy,
            true,
            100,
            "{Message:lj}{NewLine}{Exception}",
            System.Globalization.CultureInfo.InvariantCulture,
            logLevel
        );

        // Assert
        sink.Should().NotBeNull();
    }

    [Fact]
    public void Dispose_ShouldNotThrow()
    {
        // Arrange
        var sink = new TelegramBatchedSink(
            TestBotToken,
            TestChatId,
            TestProxy,
            true,
            100,
            "{Message:lj}{NewLine}{Exception}",
            System.Globalization.CultureInfo.InvariantCulture,
            LogEventLevel.Information
        );

        // Act & Assert
        sink.Invoking(s => s.Dispose()).Should().NotThrow();
    }

    private static LogEvent CreateTestLogEvent(LogEventLevel level, string messageTemplate)
    {
        // 使用Serilog的内部方法创建LogEvent
        LogEvent capturedEvent = null;

        var logger = new LoggerConfiguration()
            .WriteTo.Sink(new TestLogEventSink(e => capturedEvent = e))
            .CreateLogger();

        logger.Write(level, messageTemplate);

        return capturedEvent!;
    }

    private class TestLogEventSink : ILogEventSink
    {
        private readonly Action<LogEvent> _onEmit;

        public TestLogEventSink(Action<LogEvent> onEmit)
        {
            _onEmit = onEmit;
        }

        public void Emit(LogEvent logEvent)
        {
            _onEmit(logEvent);
        }
    }
}
