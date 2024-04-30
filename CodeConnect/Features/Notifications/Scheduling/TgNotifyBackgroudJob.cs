using CodeConnect.Data;
using Microsoft.EntityFrameworkCore;
using Quartz;
using Telegram.Bot;

namespace CodeConnect.Features.Notifications.Scheduling;

[DisallowConcurrentExecution]
public class TgNotifyBackgroudJob : IJob
{
    // Dependency Injection
    private readonly ILogger<TgNotifyBackgroudJob> _logger;
    private readonly AppDbContext _context;


    // own fields
    private readonly TelegramBotClient _bot;




    // Constructor with DI
    public TgNotifyBackgroudJob(ILogger<TgNotifyBackgroudJob> logger, AppDbContext context, IConfiguration configuration)
    {
        _logger = logger;
        _context = context;
        _bot = new TelegramBotClient(configuration["BotToken"]);
    }
    public Task Execute(IJobExecutionContext context)
    {

        var result = _context
            .Notifications
            .Include(n => n.Activity).ThenInclude(a => a.City)
            .Where(n => n.SentFirst == false || n.SentSecond == false)
            .ToList();

        var dateTimeUtc = DateTime.UtcNow;
        var dateUtc = DateOnly.FromDateTime(dateTimeUtc);
        var timeUtc = TimeOnly.FromDateTime(dateTimeUtc);
        


        foreach (var notification in result)
        {
            var actDateUtc = notification.Activity.DateUtc;
            var actTimeUtc = notification.Activity.TimeUtc;
            DateTime eventDateTimeUtc = new DateTime(
                actDateUtc.Year,
                actDateUtc.Month,
                actDateUtc.Day,
                actTimeUtc.Hour,
                actTimeUtc.Minute,
                actTimeUtc.Second,
                DateTimeKind.Utc);

            TimeSpan timeDifference = eventDateTimeUtc - dateTimeUtc;

            Console.WriteLine($"Notification about: {notification.Activity.Title} \nActivity time UTC : {eventDateTimeUtc.ToString()}\nServer current time UTC: {dateTimeUtc.ToString()}" +
                $"\nTime Difference {timeDifference.ToString()} TotalHours: {timeDifference.TotalHours}" );

            var message = $"🔔 Напоминание о мероприятии \n📝 Название: {notification.Activity.Title} \n🗓 Дата: {notification.Activity.DateLocal} \n📌Город: {notification.Activity.City.Name} \n⏱ Время {notification.Activity.TimeLocal} (местное)";

            if (notification.SentFirst == false)
            {
                if (timeDifference.TotalHours < 24 && timeDifference.TotalHours > 4)
                {
                    var newMessage = "⚡️До мероприятия меньше 24 часов\n\n" + message;
                    _bot.SendTextMessageAsync(notification.TgUserId, newMessage).Wait();
                    notification.SentFirst = true;
                }
            }

            else if (notification.SentSecond == false)
            {
                var newMessage = "⚡️⚡️⚡️До мероприятия меньше 4 часов\n\n" + message;
                if (timeDifference.TotalHours < 4)
                {
                    _bot.SendTextMessageAsync(notification.TgUserId, newMessage).Wait();
                    notification.SentSecond = true;
                }
            }
        }
        _context.SaveChanges();

        return Task.CompletedTask;
    }
}
