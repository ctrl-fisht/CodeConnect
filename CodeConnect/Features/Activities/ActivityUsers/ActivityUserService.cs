using AutoMapper;
using CodeConnect.Dto;
using CodeConnect.Data;
using CodeConnect.Entities;
using CodeConnect.Users;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CodeConnect.Features.Activities.ActivityUsers;

public class ActivityUserService
{
    private readonly IUserRepository _userRepository;
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public ActivityUserService(
        IUserRepository userRepository,
        AppDbContext context,
        IMapper mapper)
    {
        _userRepository = userRepository;
        _context = context;
        _mapper = mapper;
    }

    // add to calendar
    public async Task<CreateActivityUserResult> Create(int actId, string userName)
    {

        // получаем юзера
        var user = await _userRepository.GetUser(userName);
        if (user is null)
            return new CreateActivityUserResult
            {
                Status = CreateActivityUserStatus.UserDoesntExists
            };

        // существует ли мероприятие?
        var activity = await _context.Activities.Where(a => a.ActivityId == actId).FirstOrDefaultAsync();
        if (activity is null)
            return new CreateActivityUserResult
            {
                Status = CreateActivityUserStatus.ActivityDoesntExists
            };


        // уже существует связь ActivityUser ?
        var relation = await _context
            .ActivityUsers
            .Where(au => au.UserId == user.Id && au.ActivityId == actId)
            .FirstOrDefaultAsync();

        if (relation is not null)
            return new CreateActivityUserResult
            {
                Status = CreateActivityUserStatus.AlreadyExists
            };

        await _context.AddAsync(new ActivityUser
        {
            ActivityId = actId,
            UserId = user.Id
        });


        
        if (user.EnableTgNotif == true && user.TgUserId != null)
        {
            //var date = DateOnly.FromDateTime(DateTime.UtcNow);
            //if (date < activity.DateUtc)
                _context.Add(new Notification()
                {
                    TgUserId = user.TgUserId.Value,
                    Activity = activity,
                    SentFirst = false,
                    SentSecond = false
                });
        }

        var result = await _context.SaveChangesAsync() > 0 ? true : false;


        if (!result)
            return new CreateActivityUserResult
            {
                Status = CreateActivityUserStatus.ErrorWhileCreating
            };

        return new CreateActivityUserResult
        {
            Status = CreateActivityUserStatus.Successful
        };
    }

    public async Task<DeleteActivityUserResult> Delete(int actId, string userName)
    {
        // получаем юзера
        var user = await _userRepository.GetUser(userName);
        if (user is null)
            return new DeleteActivityUserResult
            {
                Status = DeleteActivityUserStatus.UserDoesntExists
            };

        // существует ли мероприятие?
        var community = await _context.Activities.Where(c => c.ActivityId == actId).FirstOrDefaultAsync();
        if (community is null)
            return new DeleteActivityUserResult
            {
                Status = DeleteActivityUserStatus.ActivityDoesntExists
            };


        // существует ли связь?
        var relation = await _context
            .ActivityUsers
            .Where(au => au.UserId == user.Id && au.ActivityId == actId)
            .FirstOrDefaultAsync();

        if (relation is null)
            return new DeleteActivityUserResult
            {
                Status = DeleteActivityUserStatus.DoesntExists
            };
        
        // удаление уведомления если есть
        if (user.TgUserId != null && user.EnableTgNotif == true)
        {
            var notification = _context
                .Notifications.Where(n => n.Activity.ActivityId == actId && user.TgUserId == n.TgUserId)
                .FirstOrDefault();
            if (notification != null)
                _context.Remove(notification);
        }

        _context.Remove(relation);

        var result = await _context.SaveChangesAsync() > 0 ? true : false;

        if (!result)
            return new DeleteActivityUserResult
            {
                Status = DeleteActivityUserStatus.ErrorWhileDeleting
            };

        return new DeleteActivityUserResult
        {
            Status = DeleteActivityUserStatus.Successful
        };
    }

    public async Task<bool> IsActivityInCalendar(int actId, string userName)
    {
        var user = await _userRepository.GetUser(userName);

        if (user is null)
            return false;

        return _context.ActivityUsers.Any(au => au.ActivityId == actId && au.UserId == user.Id);
    }

    public async Task<List<ActivityDto>> GetMonthActivities (int month, int year, string userName)
    {
        var user = await _userRepository.GetUser(userName);

        if (user is null)
            return new List<ActivityDto>();

        var activities = await _context
            .Activities
            .Include(a => a.City)
            .Include(a => a.ActivityCategories).ThenInclude(ac => ac.Category)
            .Include(a => a.ActivityTags).ThenInclude(at => at.Tag)
            .Include(a => a.Community)
            .Where(a => a.Members.Any(au => au.UserId == user.Id) && a.DateLocal.Year == year && a.DateLocal.Month == month)
            .OrderBy(a => a.DateUtc)
            .ThenBy(a => a.TimeUtc)
            .ToListAsync();

        return _mapper.Map<List<ActivityDto>>(activities);
        
    }

    
}
