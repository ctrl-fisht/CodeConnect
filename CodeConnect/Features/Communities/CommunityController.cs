using CodeConnect.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodeConnect.Features.Communities;

[Route("api/community")]
[ApiController]
public class CommunityController : ControllerBase
{
    private readonly CommunityService _communityService;

    public CommunityController(CommunityService communityService)
    {
        _communityService = communityService;
    }

    [Authorize]
    [HttpPost]
    [Route("")]
    public async Task<IActionResult> Create([FromBody] CreateCommunityInput input)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        Console.WriteLine();

        var result = await _communityService.Create(input, User.Identity.Name);

        switch (result.Status)
        {
            case CreateCommunityStatus.UserDoesntExists:
                return BadRequest(new { success = false,  message = "Ошибка авторизации"});
            
            case CreateCommunityStatus.ErrorWhileCreating:
                return StatusCode(500, new { success = false, message = "Ошибка во время создания сообщества" });

            case CreateCommunityStatus.AlreadyExists:
                return StatusCode(409, new { success = false, message = "Сообщество с таким именем уже существует" });

            case CreateCommunityStatus.Successful:
                return StatusCode(201, new { success = true, message = "Сообщество успешно создано" });

            // недосягаемый код т.к. бизнес логика возвращает всегда Status в пределах enum
            default: throw new ArgumentOutOfRangeException();
        }
    }



    [Authorize]
    [HttpGet]
    [Route("{commId}")]
    public async Task<IActionResult> Get(int commId)
    {
        var community = await _communityService.Get(commId);
        
        if (community is null)
            return NotFound();

        return Ok(community);
    }

    [Authorize]
    [HttpDelete]
    [Route("{commId}")]
    public async Task<IActionResult> Delete(int commId)
    {
        var result = await _communityService.Delete(commId, User.Identity.Name);

        switch (result.Status)
        {
            case DeleteCommunityStatus.Successful:
                return Ok(new { success = true, message = "Сообщество успешно удалено" });

            case DeleteCommunityStatus.UserDoesntExists:
                return BadRequest(new { success = false, message = "Ошибка авторизации" });

            case DeleteCommunityStatus.UserHasNoAccess:
                return Unauthorized(new { success = false, message = "У вас доступа к этому сообществу" });

            case DeleteCommunityStatus.CommunityDoesntExists:
                return NotFound(new { success = false, message = "Такого сообщества не существует" });

            case DeleteCommunityStatus.ErrorWhileDeleting:
                return StatusCode(500, new { status = false, message = "Ошибка во время удаления сообщества" });

            // недосягаемый код т.к. бизнес логика возвращает всегда Status в пределах enum
            default: throw new ArgumentOutOfRangeException();
        }
    }


    [Authorize]
    [HttpPatch]
    [Route("{commId}")]
    public async Task<IActionResult> UpdateGroup(int commId, [FromBody] UpdateCommunityInput input)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _communityService.Update(commId, input, User.Identity.Name);

        switch (result.Status)
        {
            case UpdateCommunityStatus.Successful:
                return Ok(new { success = true, message = "сообщество успешно обновлено" });

            case UpdateCommunityStatus.UserDoesntExists:
                return BadRequest(new { success = false, message = "Ошибка авторизации" });

            case UpdateCommunityStatus.UserHasNoAccess:
                return Unauthorized(new { success = false, message = "У вас доступа к этому сообществу" });

            case UpdateCommunityStatus.CommunityDoesntExists:
                return NotFound(new { success = false, message = "Такого сообщества не существует" });

            case UpdateCommunityStatus.ErrorWhileUpdating:
                return StatusCode(500, new { status = false, message = "Ошибка во время обновления сообщества" });

            // недосягаемый код т.к. бизнес логика возвращает всегда Status в пределах enum
            default: throw new ArgumentOutOfRangeException();
        }
    }

    [Authorize]
    [HttpGet]
    [Route("my")]
    public async Task<IActionResult> GetMyCommunities()
    {
        var result = await _communityService.GetUserCommunities(User.Identity.Name);

        return Ok(result);
    }

    [Authorize]
    [HttpGet]
    [Route("{commId}/activity")]
    public async Task<IActionResult> GetCommunityActivities(int commId)
    {
        var result = await _communityService.GetCommunityActivities(commId);

        return Ok(result);
    }

}
