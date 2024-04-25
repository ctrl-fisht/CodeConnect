using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodeConnect.Features.Telegram;

[Route("/api/tg")]
[ApiController]
public class TelegramController : ControllerBase
{
    private readonly TelegramService _telegramService;
    public TelegramController(TelegramService telegramService)
    {
        _telegramService = telegramService;
    }

    [Authorize]
    [HttpGet]
    [Route("status")]
    public async Task<IActionResult> GetTgStatus()
    {
        var status = await _telegramService.GetUserTgStatus(User.Identity.Name);

        return Ok(new { connected = status });
    }

    [Authorize]
    [HttpPost]
    [Route("startconnect")]
    public async Task<IActionResult> StartConnect([FromBody] StartTgConnectInput input)
    {
        var result = await _telegramService.StartConnect(input.Tag, User.Identity.Name);

        switch (result.Status)
        {
            case StartTgConnectStatus.UserDoesntExist:
                return Unauthorized();
            case StartTgConnectStatus.AlreadyConnected:
                return Conflict(new {success = false, message = "К этому аккаунту уже подключен телеграм"});
            case StartTgConnectStatus.CantStartSession:
                return StatusCode(500, new { success = false, message = "Ошибка при начале привязки" });
            case StartTgConnectStatus.Successful:
                return StatusCode(200, new { success = true, message = "Привязка началась", uid = result.ConnectUid });

            default: throw new ArgumentOutOfRangeException();
        }

    }


    [Authorize]
    [HttpDelete]
    [Route("remove")]
    public async Task<IActionResult> RemoveTg()
    {
        var result = await _telegramService.RemoveTg(User.Identity.Name);

        return Ok(new { success = result });
    }
}
