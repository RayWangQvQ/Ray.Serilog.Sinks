namespace Ray.Serilog.Sinks.Batched;

public static class JobContextManager
{
    private static readonly AsyncLocal<JobExecutionContext?> CurrentContext = new();

    public static JobExecutionContext? Current => CurrentContext.Value;

    public static void SetContext(JobExecutionContext context)
    {
        CurrentContext.Value = context;
    }

    public static void ClearContext()
    {
        CurrentContext.Value = null;
    }

    public static bool HasContext => CurrentContext.Value != null;
}