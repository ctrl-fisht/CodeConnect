using CodeConnect.Data;
using CodeConnect.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodeConnect.Features.Notifications;

[Route("/api/notification")]
[ApiController]
public class NotificationController : ControllerBase
{
    private readonly NotificationService _notificationService;

    public NotificationController(NotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    [Authorize]
    [HttpPost]
    [Route("tg")]
    public async Task<IActionResult> EnableTgNotifications()
    {
        var result = await _notificationService.EnableTgNotifications(User.Identity.Name);

        switch (result.Status)
        {
            case EnableTgNotificationsStatus.TgIdNull:
                return Ok(new { success = false, message = "У вас не привязан телеграм" });
            case EnableTgNotificationsStatus.Successful:
                return Ok(new { success = true, message = "Уведомления успешно включены" });
            case EnableTgNotificationsStatus.UserDoesntExist:
                return Unauthorized(new { success = false, message = "Ошибка авторизации" });
            case EnableTgNotificationsStatus.ErrorWhileEnabling:
                return StatusCode(500, new { success = false, message = "Ошибка при включении уведомлений" });
            case EnableTgNotificationsStatus.AlreadyEnabled:
                return Conflict(new { success = false, message = "Уведомления уже включены" });

            // недосягаемый код т.к. бизнес логика возвращает всегда Status в пределах enum
            default: throw new ArgumentOutOfRangeException();
        }
    }

    [Authorize]
    [HttpDelete]
    [Route("tg")]
    public async Task<IActionResult> DisableTgNotifications()
    {
        var result = await _notificationService.DisableTgNotifications(User.Identity.Name);

        switch (result.Status)
        {
            case DisableTgNotificationsStatus.Successful:
                return Ok(new { success = true, message = "Уведомления успешно выключены" });
            case DisableTgNotificationsStatus.UserDoesntExist:
                return Unauthorized(new { success = false, message = "Ошибка авторизации" });
            case DisableTgNotificationsStatus.ErrorWhileDisabled:
                return StatusCode(500, new { success = false, message = "Ошибка при выключении уведомлений" });
            case DisableTgNotificationsStatus.AlreadyDisabled:
                return Conflict(new { success = false, message = "Уведомления уже выключены" });

            // недосягаемый код т.к. бизнес логика возвращает всегда Status в пределах enum
            default: throw new ArgumentOutOfRangeException();
        }
    }

    [Authorize]
    [HttpGet]
    [Route("tg")]
    public async Task<IActionResult> GetTgNotificationsStatus()
    {
        var result = await _notificationService.GetStatus(User.Identity.Name);

        return Ok(new {enabled = result });
    }
}
