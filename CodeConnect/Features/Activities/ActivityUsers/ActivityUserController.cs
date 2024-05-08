using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodeConnect.Features.Activities.ActivityUsers;
#pragma warning disable CS8604
#pragma warning disable CS8602

[Route("api/")]
[ApiController]
public class ActivityUserController : ControllerBase
{
    private readonly ActivityUserService _activityUserService;
    public ActivityUserController(ActivityUserService activityUserService)
    {
        _activityUserService = activityUserService;
    }

    [Authorize]
    [HttpPost]
    [Route("calendar/{actId}")]
    public async Task<IActionResult> AddActivityToCalendar(int actId)
    {
        var result = await _activityUserService.Create(actId, User.Identity.Name);

        switch (result.Status)
        {
            case CreateActivityUserStatus.Successful:
                return Ok(new { success = true, message = "Вы успешно добавили мероприятие в календарь" });

            case CreateActivityUserStatus.UserDoesntExists:
                return BadRequest(new { success = false, message = "Ошибка авторизации" });

            case CreateActivityUserStatus.AlreadyExists:
                return Unauthorized(new { success = false, message = "Мероприятие уже в календаре" });

            case CreateActivityUserStatus.ActivityDoesntExists:
                return NotFound(new { success = false, message = "Такого мероприятия не существует" });

            case CreateActivityUserStatus.ErrorWhileCreating:
                return StatusCode(500, new { status = false, message = "Ошибка во время добавления мероприятия" });

            // недосягаемый код т.к. бизнес логика возвращает всегда Status в пределах enum
            default: throw new ArgumentOutOfRangeException();
        }
    }

    [Authorize]
    [HttpDelete]
    [Route("calendar/{actId}")]
    public async Task<IActionResult> RemoveActivityFromCalendar(int actId)
    {
        var result = await _activityUserService.Delete(actId, User.Identity.Name);

        switch (result.Status)
        {
            case DeleteActivityUserStatus.Successful:
                return Ok(new { success = true, message = "Вы успешно удалили мероприятие из календаря" });

            case DeleteActivityUserStatus.UserDoesntExists:
                return BadRequest(new { success = false, message = "Ошибка авторизации" });

            case DeleteActivityUserStatus.DoesntExists:
                return Unauthorized(new { success = false, message = "Мероприятия нет в календаре" });

            case DeleteActivityUserStatus.ActivityDoesntExists:
                return NotFound(new { success = false, message = "Такого мероприятия не существует" });

            case DeleteActivityUserStatus.ErrorWhileDeleting:
                return StatusCode(500, new { status = false, message = "Ошибка во время удаления мероприятия" });

            // недосягаемый код т.к. бизнес логика возвращает всегда Status в пределах enum
            default: throw new ArgumentOutOfRangeException();
        }
    }

    [Authorize]
    [HttpGet]
    [Route("activity/{actId}/incalendar")]
    public async Task<IActionResult> IsInCalendar(int actId)
    {
        return Ok(new { result = await _activityUserService.IsActivityInCalendar(actId, User.Identity.Name) });
    }

    [Authorize]
    [HttpGet]
    [Route("calendar/year/{year:int}/month/{month:int:range(1,12)}")]
    public async Task<IActionResult> GetMonthActivities(int month, int year, bool past = false)
    {
        var result = await _activityUserService.GetMonthActivities(month, year, User.Identity.Name, past);
        return Ok(result);
    }
}
#pragma warning restore CS8604
#pragma warning restore CS8602
