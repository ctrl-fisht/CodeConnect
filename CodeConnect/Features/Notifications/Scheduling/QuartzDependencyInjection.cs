using Quartz;

namespace CodeConnect.Features.Notifications.Scheduling;

public static class QuartzDependencyInjection
{
    public static void AddTgNotification(this IServiceCollection services)
    {
        services.AddQuartz(options =>
        {
            // DI Support
            options.UseMicrosoftDependencyInjectionJobFactory();


            services.ConfigureOptions<TgNotifyBackgroundJobSetup>();
        });

        services.AddQuartzHostedService(options =>
        {
            // options.WaitForJobsToComplete = true;
        });
        // hosted service ответственный за создание новых экземпляров наших
        // background-задач, когда триггер будет срабатывать 
    }
}
