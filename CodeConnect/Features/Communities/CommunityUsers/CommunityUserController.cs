using CodeConnect.Entities;
using CodeConnect.Features.Communities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
#pragma warning disable CS8604
#pragma warning disable CS8602

namespace CodeConnect.Features.Communities.CommunityUsers;

[Route("api/")]
[ApiController]
public class CommunityUserController : ControllerBase
{
    private CommunityUserService _communityUserService;

    public CommunityUserController(CommunityUserService communityUserService)
    {
        _communityUserService = communityUserService;
    }


    [Authorize]
    [HttpPost]
    [Route("community/{commId}/subscribe")]
    public async Task<IActionResult> Subscribe(int commId)
    {
        var result = await _communityUserService.Create(commId, User.Identity.Name);

        switch (result.Status)
        {
            case CreateCommunityUserStatus.Successful:
                return Ok(new { success = true, message = "Вы успешно подписались на группу" });

            case CreateCommunityUserStatus.UserDoesntExists:
                return BadRequest(new { success = false, message = "Ошибка авторизации" });

            case CreateCommunityUserStatus.AlreadyExists:
                return Unauthorized(new { success = false, message = "Вы уже подписаны на это сообщество" });

            case CreateCommunityUserStatus.CommunityDoesntExists:
                return NotFound(new { success = false, message = "Такого сообщества не существует" });

            case CreateCommunityUserStatus.ErrorWhileCreating:
                return StatusCode(500, new { status = false, message = "Ошибка во время подписки на сообщество" });

            // недосягаемый код т.к. бизнес логика возвращает всегда Status в пределах enum
            default: throw new ArgumentOutOfRangeException();
        }
    }

    [Authorize]
    [HttpDelete]
    [Route("community/{commId}/unsubscribe")]
    public async Task<IActionResult> Unsubscribe(int commId)
    {
        var result = await _communityUserService.Delete(commId, User.Identity.Name);

        switch (result.Status)
        {
            case DeleteCommunityUserStatus.Successful:
                return Ok(new { success = true, message = "Вы отписались от группы" });

            case DeleteCommunityUserStatus.UserDoesntExists:
                return BadRequest(new { success = false, message = "Ошибка авторизации" });

            case DeleteCommunityUserStatus.DoesntExists:
                return Unauthorized(new { success = false, message = "Вы не подписаны" });

            case DeleteCommunityUserStatus.CommunityDoesntExists:
                return NotFound(new { success = false, message = "Такого сообщества не существует" });

            case DeleteCommunityUserStatus.ErrorWhileDeleting:
                return StatusCode(500, new { status = false, message = "Ошибка во время отписки" });

            // недосягаемый код т.к. бизнес логика возвращает всегда Status в пределах enum
            default: throw new ArgumentOutOfRangeException();
        }
    }

    [Authorize]
    [HttpGet]
    [Route("community/subscribed")]
    public async Task<IActionResult> GetSubscribedCommunities()
    {
        return Ok(await _communityUserService.GetSubscribedCommunities(User.Identity.Name));
    }

    [Authorize]
    [HttpGet]
    [Route("community/{commId}/subscribed")]
    public async Task<IActionResult> IsSubscriberOf(int commId)
    {
        return Ok(new { result = await _communityUserService.IsSubscriber(User.Identity.Name, commId) });
    }

    [Authorize]
    [HttpGet]
    [Route("community/subscribed/activities")]
    public async Task<IActionResult> GetSubscribedActivities(int offset, int count)
    {
        var result = await _communityUserService.GetSubActivities(offset, count, User.Identity.Name);
        return Ok(result);
    }

}

#pragma warning restore CS8604
#pragma warning restore CS8602
