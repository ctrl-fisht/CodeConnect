using CodeConnect.Data;
using CodeConnect.Entities;
using CodeConnect.Users;
using Microsoft.EntityFrameworkCore;

namespace CodeConnect.Features.Notifications;

public class NotificationService
{
    private readonly IUserRepository _userRepository;
    private readonly AppDbContext _context;

    public NotificationService(IUserRepository userRepository, AppDbContext context)
    {
        _userRepository = userRepository;
        _context = context;
    }

    public async Task<EnableTgNotificationsResult> EnableTgNotifications(string username)
    {
        var user = await _userRepository.GetUser(username);

        if (user is null)
            return new EnableTgNotificationsResult
            {
                Status = EnableTgNotificationsStatus.UserDoesntExist
            };

        if (user.TgUserId == null)
            return new EnableTgNotificationsResult
            {
                Status = EnableTgNotificationsStatus.TgIdNull
            };

        if (user.EnableTgNotif == true)
            return new EnableTgNotificationsResult
            {
                Status = EnableTgNotificationsStatus.AlreadyEnabled
            };

        // добавление уведомлений по мероприятиям в списке
        var currentUtcDate = DateOnly.FromDateTime(DateTime.UtcNow);
        var activityUsers = _context
            .ActivityUsers
            .Include(au => au.Activity)
            .Where(au => au.UserId == user.Id && au.Activity.DateUtc >= currentUtcDate)
            .ToList();
        if (activityUsers.Count > 0)

            foreach (var activityUser in activityUsers)
            {
                // Здесь мы проверим, если мероприятие в день, когда пользователь включил уведомления
                // то отправим ему увед только один раз (sentFirst = true)
                var flag = false;
                
                if (currentUtcDate == activityUser.Activity.DateUtc)
                    flag = true;

                _context.Add(new Notification()
                {
                    TgUserId = user.TgUserId.Value,
                    Activity = activityUser.Activity,
                    SentFirst = flag,
                    SentSecond = false
                });
            }
        

        user.EnableTgNotif = true;
        _context.Update(user);

        var result = await _context.SaveChangesAsync() > 0 ? true : false;

        if (!result)
            return new EnableTgNotificationsResult
            {
                Status = EnableTgNotificationsStatus.ErrorWhileEnabling
            };

        return new EnableTgNotificationsResult
        {
            Status = EnableTgNotificationsStatus.Successful
        };
    }
    public async Task<DisableTgNotificationsResult> DisableTgNotifications(string username)
    {
        var user = await _userRepository.GetUser(username);

        if (user is null)
            return new DisableTgNotificationsResult
            {
                Status = DisableTgNotificationsStatus.UserDoesntExist
            };
            

        if (user.EnableTgNotif == false)
            return new DisableTgNotificationsResult
            {
                Status = DisableTgNotificationsStatus.AlreadyDisabled
            };

        // remove all notifications
        var notifications = _context.Notifications.Where(n => n.TgUserId == user.TgUserId).ToList();
        if (notifications.Count > 0)
            _context.RemoveRange(notifications);


        user.EnableTgNotif = false;
        _context.Update(user);

        var result = await _context.SaveChangesAsync() > 0 ? true : false;

        if (!result)
            return new DisableTgNotificationsResult
            {
                Status = DisableTgNotificationsStatus.ErrorWhileDisabled
            };

        return new DisableTgNotificationsResult
        {
            Status = DisableTgNotificationsStatus.Successful
        };
    }

    public async Task<bool> GetStatus(string username)
    {
        var user = await _userRepository.GetUser(username);

        if (user is null) return false;

        return user.EnableTgNotif;
    }
}
