using CodeConnect.Data;
using CodeConnect.Entities;
using CodeConnect.Users;
using Microsoft.EntityFrameworkCore;

namespace CodeConnect.Features.Telegram;

public class TelegramService
{
    private readonly AppDbContext _context;
    private readonly IUserRepository _userRepository;
    public TelegramService(AppDbContext context, IUserRepository userRepository)
    {
        _context = context;
        _userRepository = userRepository;
    }

    public async Task<bool> GetUserTgStatus(string username)
    {
        var user = await _userRepository.GetUser(username);

        if (user is null)
            return false;

        if (user.TgUserId is null)
            return false;

        return true;

    }

    public async Task<StartTgConnectResult> StartConnect(string tag, string username)
    {
        var user = await _userRepository.GetUser(username);

        if (user is null)
            return new StartTgConnectResult
            {
                Status = StartTgConnectStatus.UserDoesntExist
            };

        if (user.TgUserId is not null)
            return new StartTgConnectResult
            {
                Status = StartTgConnectStatus.AlreadyConnected
            };

        if (tag.Contains('@'))
            tag = tag.Replace("@", "");

        var connectSession = new TelegramConnection()
        {
            UserId = user.Id,
            Tag = tag,
        };

        _context.Add(connectSession);

        var result = await _context.SaveChangesAsync() > 0 ? true : false;

        if (!result)
            return new StartTgConnectResult
            {
                Status = StartTgConnectStatus.CantStartSession
            };

        return new StartTgConnectResult
        {
            Status = StartTgConnectStatus.Successful,
            ConnectUid = connectSession.TelegramConnectionId.ToString()
        };
    }
    public async Task<bool> RemoveTg(string username)
    {
        return await _userRepository.RemoveTgId(username);
    }
}
