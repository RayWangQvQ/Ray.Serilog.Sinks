using System.Collections.Concurrent;

namespace Ray.Serilog.Sinks.Batched;

public class BatchSinkManager
{
    private static readonly ConcurrentDictionary<IBatchSink, bool> Sinks = new();

    internal static void RegisterSink(IBatchSink sink)
    {
        Sinks.TryAdd(sink, true);
    }

    internal static void UnregisterSink(IBatchSink sink)
    {
        Sinks.TryRemove(sink, out _);
    }

    public static async Task FlushAllAsync()
    {
        var tasks = new List<Task>();

        foreach (var sink in Sinks.Keys)
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
