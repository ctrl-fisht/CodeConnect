using Microsoft.Extensions.Options;
using Quartz;

namespace CodeConnect.Features.Notifications.Scheduling;

public class TgNotifyBackgroundJobSetup : IConfigureOptions<QuartzOptions>
{
    public void Configure(QuartzOptions options)
    {
        var jobKey = JobKey.Create(nameof(TgNotifyBackgroudJob));
        options
            .AddJob<TgNotifyBackgroudJob>(jobBuilder => jobBuilder.WithIdentity(jobKey))
            .AddTrigger(trigger =>
                trigger
                    .ForJob(jobKey)
                    .WithSimpleSchedule(schedule =>
                        schedule.WithIntervalInSeconds(60).RepeatForever()));
    }
}