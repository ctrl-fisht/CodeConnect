using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodeConnect.Features.Tags;

[Route("api/tag")]
[ApiController]
public class TagController : ControllerBase
{
    private readonly TagService _tagService;
    public TagController(TagService tagService)
    {
        _tagService = tagService;
    }


    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetTagsList()
    {
        return Ok(await _tagService.GetTagsList());
    }
}
