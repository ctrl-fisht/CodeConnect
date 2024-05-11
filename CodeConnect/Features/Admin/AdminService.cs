using AutoMapper;
using CodeConnect.Data;
using CodeConnect.Dto;
using Microsoft.EntityFrameworkCore;

namespace CodeConnect.Features.Admin;

public class AdminService
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public AdminService(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<bool> ApproveActivity(int actId)
    {
        var activity = await _context.Activities.IgnoreQueryFilters().Where(a => a.ActivityId == actId).FirstOrDefaultAsync();

        if (activity is null)
            return false;

        activity.IsActive = true;

        return await _context.SaveChangesAsync() > 0 ? true : false;
    }

    public async Task<List<ActivityDto>> GetActivities()
    {
        var activities = await _context
            .Activities.IgnoreQueryFilters()
            .Include(a => a.Community)
            .Include(a => a.City)
            .Where(a => a.Deleted != true && a.Declined != true)
            .Where(a => a.IsActive == false)
            .ToListAsync();

        return _mapper.Map<List<ActivityDto>>(activities);
    }

    public async Task<bool> DeclineActivity(int actId)
    {
        var activity = await _context.Activities.IgnoreQueryFilters().Where(a => a.ActivityId == actId).FirstOrDefaultAsync();

        if (activity is null)
            return false;

        activity.Declined = true;

        return await _context.SaveChangesAsync() > 0 ? true : false;
    }
}
