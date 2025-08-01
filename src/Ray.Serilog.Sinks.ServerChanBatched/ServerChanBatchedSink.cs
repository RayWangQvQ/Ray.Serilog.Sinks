﻿using Ray.Serilog.Sinks.Batched;
using Serilog.Events;

namespace Ray.Serilog.Sinks.ServerChanBatched;

public class ServerChanBatchedSink : BatchedSink
{
    private readonly string _scKey;
    private readonly string _turboScKey;

    public ServerChanBatchedSink(
        string scKey,
        string turboScKey,
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
        _scKey = scKey;
        _turboScKey = turboScKey;
    }

    public override void Emit(LogEvent logEvent)
    {
        if (_scKey.IsNullOrEmpty() && _turboScKey.IsNullOrEmpty())
            return;
        base.Emit(logEvent);
    }

    protected override PushService PushService
    {
        get
        {
            if (!_turboScKey.IsNullOrEmpty())
                return new ServerChanTurboApiClient(_turboScKey);
            return new ServerChanApiClient(_scKey);
        }
    }
}
