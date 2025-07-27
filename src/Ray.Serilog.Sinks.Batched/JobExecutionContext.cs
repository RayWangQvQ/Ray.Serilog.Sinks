namespace Ray.Serilog.Sinks.Batched;

public class JobExecutionContext
{
    public string JobId { get; }
    public string JobName { get; }
    public DateTime StartTime { get; }

    public JobExecutionContext(string jobId, string jobName)
    {
        JobId = jobId ?? throw new ArgumentNullException(nameof(jobId));
        JobName = jobName ?? throw new ArgumentNullException(nameof(jobName));
        StartTime = DateTime.Now;
    }
}