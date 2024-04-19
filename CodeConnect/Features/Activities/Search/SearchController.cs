using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodeConnect.Features.Activities.Search;

[Route("api/activity")]
[ApiController]
public class SearchController : ControllerBase
{
    private readonly SearchService _searchService;

    public SearchController(SearchService searchService)
    {
        _searchService = searchService;
    }


    [Route("filter")]
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> SearchActivities([FromBody] SearchActivityInput input, int offset, int count)
    {
        var result = await _searchService.SearchActivities(input, offset, count);

        return Ok(result);
    }
}
