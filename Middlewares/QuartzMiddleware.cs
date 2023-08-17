
using PostIt.Jobs;
using Quartz;

namespace PostIt.Middlewares;

public static class QuartzMiddleware
{
    public static void AddQuartzMiddleware(this IServiceCollection services)
    {
        services.AddQuartz(quartz =>
        {
            quartz.SchedulerId = "Scheduler-PostIt";
            quartz.UseMicrosoftDependencyInjectionJobFactory();

            quartz.UseSimpleTypeLoader();
            quartz.UseInMemoryStore();
            quartz.UseDefaultThreadPool(tp =>
            {
                tp.MaxConcurrency = 2;
            });

            var checkDBKey = new JobKey(CheckDBJob.KeyName, CheckDBJob.GroupName);
            quartz.AddJob<CheckDBJob>(jobConfigurator => jobConfigurator.StoreDurably().WithIdentity(checkDBKey).WithDescription(CheckDBJob.JobDescription));
            quartz.AddTrigger(triggerConfigurator => triggerConfigurator
                .WithIdentity(CheckDBJob.TriggerName)
                .ForJob(checkDBKey)
                .StartAt(DateBuilder.EvenSecondDate(DateTimeOffset.UtcNow.AddSeconds(CheckDBJob.Delay)))
                .WithDescription(CheckDBJob.TriggerDescription)
            );

        });

        services.AddQuartzHostedService(options =>
        {
            options.WaitForJobsToComplete = true;
        });
    }
}
