using CodeConnect.Data;
using CodeConnect.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CodeConnect.TelegramBot;

public class TelegramBot
{
    private readonly TelegramBotClient _botClient;
    private readonly IServiceScopeFactory _scopeFactory;

    public TelegramBot(string botToken, IServiceScopeFactory scopeFactory)
    {
        _botClient = new TelegramBotClient(botToken);
        _scopeFactory = scopeFactory;
    }
        
    public async Task StartLongPolling()
    {
        Console.WriteLine("**** TG BOT ****");
        Console.WriteLine("[TgBot] Starting Long Polling");
        Console.WriteLine($"[TgBot] Bot Polling Status: {_botClient.GetMeAsync().Result.IsBot}");
        Console.WriteLine("**** TG BOT ****");

        int offset = 0;

        while (true)
        {
            var updates = await _botClient.GetUpdatesAsync(offset);

            foreach (var update in updates)
            {
                if (update.Message != null && update.Message.Text.StartsWith("/start"))
                {
                    await HandleStartCommand(update.Message);
                }
                offset = update.Id + 1;
            }
        }
    }
    private async Task HandleStartCommand(Message message)
    {
        // пропуск обычных start комманд без payload
        if (message.Text.Length == 6)
            return;

        var connectUid = message.Text.Split(" ")[1];

        var msg = await _botClient.SendTextMessageAsync(message.Chat.Id, "Проверяем....");

        Guid guidParsed;
        if (!Guid.TryParse(connectUid, out guidParsed))
        {
            await _botClient.EditMessageTextAsync(msg.Chat.Id, msg.MessageId, "Неудачная попытка, попробуйте всё сначала");
            return;
        }

        // Get TgConnection DATA
        TelegramConnection? telegramConnection;
        using (var scope = _scopeFactory.CreateScope())
        {
            var context = scope.ServiceProvider.GetService<AppDbContext>();
            telegramConnection = context
                .TelegramConnections
                .Where(tc => tc.TelegramConnectionId == guidParsed)
                .FirstOrDefault();
        }
        if (telegramConnection is null)
            await _botClient.EditMessageTextAsync(msg.Chat.Id, msg.MessageId, "Неудачная попытка, попробуйте всё сначала");
        
        if (!telegramConnection.Tag.Equals(message.From.Username))
        {
            await _botClient.EditMessageTextAsync(msg.Chat.Id, msg.MessageId, "Неправильно указано тэг телеграм, вернитесь на сайт и попробуйте еще раз");
            return;
        }


        bool result;
        // Добавляем юзеру ТГ IDшник
        using (var scope = _scopeFactory.CreateScope())
        {
            var context = scope.ServiceProvider.GetService<AppDbContext>();

            var user = context.AppUsers.Where(u => u.Id == telegramConnection.UserId).First();

            user.TgUserId = message.From.Id;
            context.Update(user);
            result = await context.SaveChangesAsync() > 0 ? true : false ;
        }

        if (!result)
        {
            await _botClient.EditMessageTextAsync(msg.Chat.Id, msg.MessageId, "Ошибка при привязке, попробуйте снова");
            return;
        }

        await _botClient.EditMessageTextAsync(msg.Chat.Id, msg.MessageId, "Успешно, можете обновить страницу.");



    }
}
