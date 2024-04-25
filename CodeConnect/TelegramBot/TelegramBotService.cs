using CodeConnect.Data;

namespace CodeConnect.TelegramBot;


public class TelegramBotService : BackgroundService
{
    private readonly TelegramBot _bot;
    private readonly IServiceScopeFactory _scopeFactory;

    public TelegramBotService(IConfiguration configuration, IServiceScopeFactory scopeFactory)
    {
        _bot = new TelegramBot(configuration["BotToken"], scopeFactory);
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _bot.StartLongPolling();
    }
}

