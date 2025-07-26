using Serilog.Events;

namespace Ray.Serilog.Sinks.Batched;

public interface IBatchSink
{
    void Emit(LogEvent logEvent);

    Task FlushAsync();

    bool IsDisposed { get; }
}
