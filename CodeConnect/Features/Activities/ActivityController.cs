using CodeConnect.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodeConnect.Features.Activities;

[Route("api/activity")]
[ApiController]
public class ActivityController : ControllerBase
{
    private readonly ActivityService _activityService;

    public ActivityController(ActivityService activityService)
    {
        _activityService = activityService;
    }

    [HttpGet]
    [Route("{actId}")]
    public async Task<IActionResult> GetActivityById(int actId)
    {
        // авторизован ли пользователь, если да то мероприятие без времени пользователя
        ActivityDto? activity = await _activityService.Get(actId);
        

        if (activity is null)
            return NotFound();

        return Ok(activity);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateActivity([FromBody] CreateActivityInput input)
    {


        var result = await _activityService.CreateActivity(input, User.Identity.Name);

        switch (result.Status)
        {
            case CreateActivityStatus.UserDoesntExist:
                return BadRequest(new { success = false, message = "Ошибка авторизации" });

            case CreateActivityStatus.ErrorWhileCreating:
                return StatusCode(500, new { success = false, message = "Ошибка во время создания мероприятия" });

            case CreateActivityStatus.CityDoesntExist:
                return StatusCode(409, new { success = false, message = "Указанного города нет в базе" });

            case CreateActivityStatus.Successful:
                return StatusCode(201, new { success = true, message = "Мероприятие успешно создано" });

            case CreateActivityStatus.CommunityDoesntExist:
                return StatusCode(409, new { success = true, message = "Указанного сообщества нет в базе" });

            case CreateActivityStatus.UserHasNoAccess:
                return StatusCode(401, new { success = true, message = "У вас нет доступа к этому сообществу" });

            case CreateActivityStatus.BadTags:
                return StatusCode(409, new { success = true, message = "Таких тэгов нету в базе" });

            case CreateActivityStatus.BadCategories:
                return StatusCode(409, new { success = true, message = "Таких категорий нету в базе" });


            // недосягаемый код т.к. бизнес логика возвращает всегда Status в пределах enum
            default: throw new ArgumentOutOfRangeException();
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetActivities(int offset, int count)
    {
        var activities = await _activityService.GetActivityList(offset, count);

        return Ok(activities);
    }

    [Route("/past")]
    [HttpGet]
    public async Task<IActionResult> GetActivitiesPast(int offset, int count)
    {
        var activities = await _activityService.GetActivityListPast(offset, count);
        return Ok(activities);
    }


    [Authorize]
    [HttpPatch]
    [Route("{actId}")]
    public async Task<IActionResult> UpdateEvent(int actId, UpdateActivityInput input)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);


        var result = await _activityService.UpdateActivity(actId, input, User.Identity.Name);

        switch (result.Status)
        {
            case UpdateActivityStatus.UserDoesntExist:
                return BadRequest(new { success = false, message = "Ошибка авторизации" });

            case UpdateActivityStatus.ErrorWhileUpdating:
                return StatusCode(500, new { success = false, message = "Ошибка во время обновления мероприятия" });

            case UpdateActivityStatus.ActivityDoesntExist:
                return NotFound(new { success = false, message = "Данного мероприятия не существует" });

            case UpdateActivityStatus.CityDoesntExist:
                return StatusCode(409, new { success = false, message = "Указанного города нет в базе" });

            case UpdateActivityStatus.Successful:
                return StatusCode(201, new { success = true, message = "Мероприятие успешно обновлено" });

            case UpdateActivityStatus.CommunityDoesntExist:
                return StatusCode(409, new { success = true, message = "Указанного сообщества нет в базе" });

            case UpdateActivityStatus.UserHasNoAccess:
                return StatusCode(401, new { success = true, message = "У вас нет доступа к этому мероприятию или сообществу" });

            case UpdateActivityStatus.BadTags:
                return StatusCode(409, new { success = true, message = "Таких тэгов нету в базе" });

            case UpdateActivityStatus.BadCategories:
                return StatusCode(409, new { success = true, message = "Таких категорий нету в базе" });


            // недосягаемый код т.к. бизнес логика возвращает всегда Status в пределах enum
            default: throw new ArgumentOutOfRangeException();
        }
    }

    [Authorize]
    [HttpDelete]
    [Route("{actId}")]
    public async Task<IActionResult> DeleteActivity(int actId)
    {
        var result = await _activityService.DeleteActivity(actId, User.Identity.Name);

        switch (result.Status)
        {
            case DeleteActivityStatus.UserDoesntExist:
                return BadRequest(new { success = false, message = "Ошибка авторизации" });

            case DeleteActivityStatus.ErrorWhileDeleting:
                return StatusCode(500, new { success = false, message = "Ошибка во время удаления" });

            case DeleteActivityStatus.ActivityDoesntExist:
                return NotFound(new { success = false, message = "Данного мероприятия не существует" });

            case DeleteActivityStatus.Successful:
                return StatusCode(201, new { success = true, message = "Мероприятие успешно обновлено" });

            case DeleteActivityStatus.UserHasNoAccess:
                return StatusCode(401, new { success = true, message = "У вас нет доступа к этому мероприятию или сообществу" });

            // недосягаемый код т.к. бизнес логика возвращает всегда Status в пределах enum
            default: throw new ArgumentOutOfRangeException();
        }
    }

    [Authorize]
    [HttpGet]
    [Route("nearest")]
    public async Task<IActionResult> GetNearestActivities(int offset, int count)
    {
        var result = await _activityService.GetNearest(offset, count);

        if (result is null)
            return NotFound();

        return Ok(result);
        
    }

    [Authorize]
    [HttpGet]
    [Route("my")]
    public async Task<IActionResult> GetMyActivities(bool past = false)
    {
        var result = await _activityService.GetUserActivities(User.Identity.Name, past);

        return Ok(result);
    }

    [Authorize]
    [HttpPut]
    [Route("{actId}/image/small")]
    public async Task<IActionResult> UpdateSmallImage(int actId, IFormFile file)
    {
        var result = await _activityService.UpdateSmallPhoto(actId, file, User.Identity.Name);

        switch (result.Status)
        {
            case UpdateActivitySmallPhotoStatus.UserDoesntExist:
                return BadRequest(new { success = false, message = "Ошибка авторизации" });

            case UpdateActivitySmallPhotoStatus.ErrorWhileUpdating:
                return StatusCode(500, new { success = false, message = "Ошибка во время обновления" });

            case UpdateActivitySmallPhotoStatus.ActivityDoesntExist:
                return NotFound(new { success = false, message = "Данного мероприятия не существует" });

            case UpdateActivitySmallPhotoStatus.Successful:
                return StatusCode(201, new { success = true, message = "Фото успешно обновлено" });

            case UpdateActivitySmallPhotoStatus.IncorrectFormat:
                return BadRequest(new { success = false, message = "Неправильный формат файла, поддерживаются только изображения" });

            case UpdateActivitySmallPhotoStatus.FileTooBig:
                return BadRequest(new { success = false, message = "Размер файла не должен превышать 5 МБ" });

            case UpdateActivitySmallPhotoStatus.UserHasNoAccess:
                return StatusCode(401, new { success = true, message = "У вас нет доступа к этому мероприятию" });

            // недосягаемый код т.к. бизнес логика возвращает всегда Status в пределах enum
            default: throw new ArgumentOutOfRangeException();
        }
    }

    [Authorize]
    [HttpPut]
    [Route("{actId}/image/banner")]
    public async Task<IActionResult> UpdateBannerImage(int actId, IFormFile file)
    {
        var result = await _activityService.UpdateBannerPhoto(actId, file, User.Identity.Name);

        switch (result.Status)
        {
            case UpdateActivityBannerPhotoStatus.UserDoesntExist:
                return BadRequest(new { success = false, message = "Ошибка авторизации" });

            case UpdateActivityBannerPhotoStatus.ErrorWhileUpdating:
                return StatusCode(500, new { success = false, message = "Ошибка во время обновления" });

            case UpdateActivityBannerPhotoStatus.ActivityDoesntExist:
                return NotFound(new { success = false, message = "Данного мероприятия не существует" });

            case UpdateActivityBannerPhotoStatus.Successful:
                return StatusCode(201, new { success = true, message = "Фото успешно обновлено" });

            case UpdateActivityBannerPhotoStatus.IncorrectFormat:
                return BadRequest(new { success = false, message = "Неправильный формат файла, поддерживаются только изображения" });

            case UpdateActivityBannerPhotoStatus.FileTooBig:
                return BadRequest(new { success = false, message = "Размер файла не должен превышать 5 МБ" });

            case UpdateActivityBannerPhotoStatus.UserHasNoAccess:
                return StatusCode(401, new { success = true, message = "У вас нет доступа к этому мероприятию" });

            // недосягаемый код т.к. бизнес логика возвращает всегда Status в пределах enum
            default: throw new ArgumentOutOfRangeException();
        }
    }

    [Authorize]
    [HttpGet]
    [Route("{actId}/my")]
    public async Task<IActionResult> IsMyActivity(int actId)
    {
        var result = _activityService.IsUserActivity(User.Identity.Name, actId);

        return Ok(new { result = result });
    }
}
