using CodeConnect.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodeConnect.Features.Admin;

[Route("/api/admin")]
[ApiController]
public class AdminController : ControllerBase
{
    private readonly AdminService _adminService;

    public AdminController(AdminService adminService)
    {
        _adminService = adminService;
    }

    [Authorize(Roles = UserRoles.Admin)]
    [HttpPost]
    [Route("approve/{actId}")]
    public async Task<IActionResult> ApproveActivity(int actId)
    {
        var result = _adminService.ApproveActivity(actId);
        return Ok(new {result = result});
    }

    [Authorize(Roles = UserRoles.Admin)]
    [HttpPost]
    [Route("decline/{actId}")]
    public async Task<IActionResult> DeclineActivity(int actId)
    {
        var result = _adminService.DeclineActivity(actId);
        return Ok(new { result = result });
    }

    [Authorize(Roles = UserRoles.Admin)]
    [HttpGet]
    [Route("activities")]
    public async Task<IActionResult> GetActivities()
    {
        var result = await _adminService.GetActivities();
        return Ok(result);
    }
}
