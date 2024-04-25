using AutoMapper;
using CodeConnect.Dto;
using CodeConnect.Data;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.Query.ExpressionTranslators.Internal;

namespace CodeConnect.Features.Activities.Search;

public class SearchService
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    public SearchService(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<ActivityDto>> SearchActivities(SearchActivityInput input, int offset, int count)
    {
        var query = _context.Activities.AsQueryable();
        
        // Name
        if (input.Title != null)
        {
            query = query.Where(a => a.Title.ToLower().Contains(input.Title.ToLower()));
        }
        // DateLocal
        if (input.DateLocal != null)
        {
            query = query.Where(a => a.DateLocal.Equals(input.DateLocal));
        }
        // City
        if (input.CityId != null)
        {
            query = query.Where(a => a.City.CityId == input.CityId);
        }
        // HasStream
        if (input.HasStream!= null)
        {
            query = query.Where(a => a.HasStream == input.HasStream);
        }

        // HasFreeTicket?
        if (input.FreeTicket != null)
        {
            if (input.FreeTicket.Value)
                query = query.Where(a => a.TicketPrice == 0);
            else
                query = query.Where(a => a.TicketPrice != 0);
        }

        // Tags
        if (input.TagsIds != null && input.TagsIds.Count > 0)
        {
            query = query.Where(a => a.ActivityTags.Any(at => input.TagsIds.Contains(at.TagId)));
        }

        // Categories
        if (input.CategoriesIds != null && input.CategoriesIds.Count > 0)
        {
            query = query.Where(a => a.ActivityCategories.Any(ac => input.CategoriesIds.Contains(ac.CategoryId)));
        }

        var activities = await query
            .Skip(offset)
            .Take(count)
            .Include(a => a.ActivityCategories).ThenInclude(ac => ac.Category)
            .Include(a => a.ActivityTags).ThenInclude(at => at.Tag)
            .Include(a => a.City)
            .Include(a => a.Image)
            .Include(a => a.Community)
            .ToListAsync();

        return _mapper.Map<List<ActivityDto>>(activities);

    }
}
