using Quartz;

namespace PostIt.Jobs;

[DisallowConcurrentExecution]
public class CheckDBJob : IJob
{
    public const string KeyName = "CreateDB";
    public const string GroupName = $"{KeyName}Group";
    public const string TriggerName = $"{KeyName}Trigger";
    public const string JobDescription = $"";
    public const string TriggerDescription = $"";

    public const int Interval = 0;
    public const int Delay = 15;

    private readonly IServiceProvider _service;

    public CheckDBJob(IServiceProvider service)
    {
        _service = service;
    }

    public virtual async Task Execute(IJobExecutionContext context)
    {
        JobKey jobKey = context.JobDetail.Key;
        Log.Verbose($"Executando tarefa {jobKey.Name}");

        try
        {
            var service = _service.GetRequiredService<ICheckDBService>();
            await service.Check();
        }
        catch (Exception e)
        {
            Log.Error($"{e.Message}");
        }
    }
}