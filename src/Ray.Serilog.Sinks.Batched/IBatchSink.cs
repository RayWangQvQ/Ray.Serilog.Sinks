using Serilog.Events;

namespace Ray.Serilog.Sinks.Batched;

public interface IBatchSink
{
    void Emit(LogEvent logEvent);

    Task FlushAsync(string title = "");

    bool IsDisposed { get; }
}
