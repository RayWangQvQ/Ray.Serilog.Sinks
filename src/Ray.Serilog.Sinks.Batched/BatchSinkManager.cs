using System.Collections.Concurrent;

namespace Ray.Serilog.Sinks.Batched;

public class BatchSinkManager
{
    private static readonly ConcurrentBag<IBatchSink> Sinks = new();

    internal static void RegisterSink(IBatchSink sink)
    {
        Sinks.Add(sink);
    }

    internal static void UnregisterSink(IBatchSink sink)
    {
        // ConcurrentBag不支持直接移除，但在Dispose时会自动清理
    }

    public static async Task FlushAllAsync()
    {
        var tasks = new List<Task>();

        foreach (var sink in Sinks)
        {
            if (!sink.IsDisposed)
            {
                tasks.Add(sink.FlushAsync());
            }
        }

        if (tasks.Count > 0)
        {
            await Task.WhenAll(tasks);
        }
    }

    public static int RegisteredSinkCount => Sinks.Count;
}