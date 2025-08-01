﻿using Ray.Serilog.Sinks.Batched;
using Serilog.Events;

namespace Ray.Serilog.Sinks.TelegramBatched;

public class TelegramBatchedSink : BatchedSink
{
    private readonly string _botToken;
    private readonly string _chatId;
    private readonly string _proxy;

    public TelegramBatchedSink(
        string botToken,
        string chatId,
        string proxy,
        bool sendBatchesAsOneMessages,
        int batchSizeLimit,
        string outputTemplate,
        IFormatProvider formatProvider,
        LogEventLevel minimumLogEventLevel
    )
        : base(
            sendBatchesAsOneMessages,
            batchSizeLimit,
            outputTemplate: outputTemplate,
            formatProvider: formatProvider,
            minimumLogEventLevel: minimumLogEventLevel
        )
    {
        _botToken = botToken;
        _chatId = chatId;
        _proxy = proxy;
    }

    public override void Emit(LogEvent logEvent)
    {
        if (_botToken.IsNullOrEmpty() | _chatId.IsNullOrEmpty())
            return;
        base.Emit(logEvent);
    }

    protected override PushService PushService =>
        new TelegramApiClient(_botToken, _chatId, _proxy, 5);
}
