using AutoMapper;
using CodeConnect.Data;
using CodeConnect.Entities;
using CodeConnect.Users;
using Microsoft.EntityFrameworkCore;


namespace CodeConnect.Features.Activities;

public class ActivityService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly AppDbContext _context;
    public ActivityService(IUserRepository userRepository, IMapper mapper, AppDbContext context)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _context = context;
    }

    public async Task<ActivityDto?> Get(int actId)
    {

        var activity = await _context
            .Activities
            .Include(a => a.City)
            .Include(a => a.ActivityCategories).ThenInclude(ac => ac.Category)
            .Include(a => a.ActivityTags).ThenInclude(at => at.Tag)
            .Include(a => a.Community)
            .Where(a => a.ActivityId == actId).FirstOrDefaultAsync();
        
        if (activity is null)
            return null;

        return _mapper.Map<ActivityDto>(activity);

    }

    public async Task<List<ActivityDto>?> GetNearest(int offset, int count)
    {
        var activities = await _context
            .Activities
            .Skip(offset)
            .Take(count)
            .Include(a => a.City)
            .Include(a => a.Community)
            .Include(a => a.ActivityCategories).ThenInclude(ac => ac.Category)
            .Include(a => a.ActivityTags).ThenInclude(at => at.Tag)
            .OrderBy(a => a.DateUtc)
            .ToListAsync();

        if (activities is null)
            return null;

        return _mapper.Map<List<ActivityDto>>(activities);

    }

    public async Task<CreateActivityResult> CreateActivity(CreateActivityInput input, string username)
    {
        var user = await _userRepository.GetUser(username);
        if (user is null)
            return new CreateActivityResult
            {
                Status = CreateActivityStatus.UserDoesntExist
            };

        // проверка города (есть ли в базе по CityId)?
        var city = await _context.Cities.FindAsync(input.CityId);

        if (city is null)
            return new CreateActivityResult
            {
                Status = CreateActivityStatus.CityDoesntExist
            };

        // проверка комьюнити - есть ли в базе?
        var community = await _context.Communities.FindAsync(input.CommunityId);
        if (community is null)
            return new CreateActivityResult
            {
                Status = CreateActivityStatus.CommunityDoesntExist
            };

        // есть ли у пользователя доступ к этой группе
        if (community.Owner.Id != user.Id)
            return new CreateActivityResult
            {
                Status = CreateActivityStatus.UserHasNoAccess
            };


        // Проверка тегов
        var tags = new List<Tag>();
        foreach (var tagId in input.TagsIds)
        {
            var tag = await _context.Tags.FindAsync(tagId);
            if (tag is null)
                return new CreateActivityResult
                {
                    Status = CreateActivityStatus.BadTags
                };
            tags.Add(tag);
        }

        // Проверка категорий
        var categories = new List<Category>();
        foreach (var categoryId in input.CategoriesIds)
        {
            var category = await _context.Categories.FindAsync(categoryId);
            if (category is null)
                return new CreateActivityResult
                {
                    Status = CreateActivityStatus.BadCategories
                };
        }

        var newActivity = _mapper.Map<Activity>(input);

        // считаем из локального времени + часового пояса города в котором мероприятие - UTC время и дату
        var utcDateAndTime = TimeConverter.CalculateUtcFromLocal(newActivity.DateLocal, newActivity.TimeLocal, city.UtcOffsetHours);
        newActivity.TimeUtc = utcDateAndTime.Item2;
        newActivity.DateUtc = utcDateAndTime.Item1;
        
        newActivity.IsActive = true;

        newActivity.City = city;

        

        newActivity.Owner = user;
        newActivity.Community = community;

        foreach (var tag in tags)
        {
            newActivity.ActivityTags.Add(new ActivityTag { Activity = newActivity, Tag = tag });
        }

        foreach (var category in categories)
        {
            newActivity.ActivityCategories.Add(new ActivityCategory { Activity = newActivity, Category = category });
        }

        // creating
        await _context.AddAsync(newActivity);
        var result = await _context.SaveChangesAsync() > 0 ? true : false;
        if (!result)
            return new CreateActivityResult
            {
                Status = CreateActivityStatus.ErrorWhileCreating
            };

        return new CreateActivityResult
        {
            Status = CreateActivityStatus.Successful
        };
    }

    public async Task<List<ActivityDto>> GetActivityList(int offset, int count)
    {
        var activities = await _context
            .Activities
            .Skip(offset)
            .Take(count)
            .Include(a => a.City)
            .Include(a => a.Community)
            .Include(a => a.ActivityCategories).ThenInclude(ac => ac.Category)
            .Include(a => a.ActivityTags).ThenInclude(at => at.Tag)
            .OrderBy(a => a.DateUtc)
            .ToListAsync();

        return _mapper.Map<List<ActivityDto>>(activities);


    }

    public async Task<UpdateActivityResult> UpdateActivity(int actId, UpdateActivityInput input, string username)
    {
        var user = await _userRepository.GetUser(username);
        if (user is null)
            return new UpdateActivityResult
            {
                Status = UpdateActivityStatus.UserDoesntExist
            };
        
        var activity = await _context
            .Activities
            .Where(a => a.ActivityId == actId)
            .Include(a => a.Owner)
            .Include(a => a.City)
            .Include(a => a.Community)
            .Include(a => a.ActivityCategories).ThenInclude(ac => ac.Category)
            .Include(a => a.ActivityTags).ThenInclude(at => at.Tag)
            .FirstOrDefaultAsync();

        if (activity is null)
        {
            return new UpdateActivityResult
            {
                Status = UpdateActivityStatus.ActivityDoesntExist
            };
        }

        if (activity.Owner.Id != user.Id)
        {
            return new UpdateActivityResult
            {
                Status = UpdateActivityStatus.UserHasNoAccess
            };
        }


        City? city = null;
        if (input.CityId is not null)
        {
            // проверка города (есть ли в базе по CityId)?
            city = await _context.Cities.FindAsync(input.CityId);

            if (city is null)
                return new UpdateActivityResult
                {
                    Status = UpdateActivityStatus.CityDoesntExist
                };
        }

        Community? community = null;
        if (input.CommunityId is not null)
        {
            // проверка комьюнити - есть ли в базе?
            community = await _context.Communities.FindAsync(input.CommunityId);
            if (community is null)
                return new UpdateActivityResult
                {
                    Status = UpdateActivityStatus.CommunityDoesntExist
                };

            // есть ли у пользователя доступ к этой группе
            if (community.Owner.Id != user.Id)
                return new UpdateActivityResult
                {
                    Status = UpdateActivityStatus.UserHasNoAccess
                };

        }
        List<Tag> tags = null!;
        if (input.TagsIds != null && input.TagsIds.Count > 0)
        {
            // Проверка тегов
            tags = new List<Tag>();
            foreach (var tagId in input.TagsIds)
            {
                var tag = await _context.Tags.FindAsync(tagId);
                if (tag is null)
                    return new UpdateActivityResult
                    {
                        Status = UpdateActivityStatus.BadTags
                    };
                tags.Add(tag);
            }
        }
        List<Category> categories = null!;
        if (input.CategoriesIds != null && input.CategoriesIds.Count > 0)
        {
            // Проверка категорий
            categories = new List<Category>();
            foreach (var categoryId in input.CategoriesIds)
            {
                var category = await _context.Categories.FindAsync(categoryId);
                if (category is null)
                    return new UpdateActivityResult
                    {
                        Status = UpdateActivityStatus.BadCategories
                    };
                categories.Add(category);
            }
        }
        
        if (input.Title != null)
            activity.Title = input.Title;


        if (input.DateLocal != null)
        {
            activity.DateUtc = activity.DateLocal;
            activity.DateLocal = (DateOnly)input.DateLocal;
        }


        if (input.TimeLocal != null)
        {
            activity.TimeLocal = (TimeOnly)input.TimeLocal;
            activity.TimeUtc = activity.TimeLocal.AddHours(activity.City.UtcOffsetHours);
        }

        if (input.CityId != null)
            activity.City = city;

        if (input.CommunityId != null)
            activity.Community = community;

        if (input.Description != null)
            activity.Description = input.Description;

        if (input.Address != null)
            activity.Address = input.Address;

        if (input.DurationMinutes != null)
            activity.DurationMinutes = (int)input.DurationMinutes;

        if (input.HasStream != null)
            activity.HasStream = (bool)input.HasStream;

        if (input.StreamURL != null)
            activity.StreamURL = input.StreamURL;

        if (input.TicketPrice != null)
            activity.TicketPrice = (int)input.TicketPrice;

        if (input.WebsiteURL != null)
            activity.WebsiteURL = input.WebsiteURL;
        
        if (input.TagsIds != null && input.TagsIds.Count > 0)
        {
            activity.ActivityTags = new List<ActivityTag>();
            foreach (var tag in tags)
            {
                activity.ActivityTags.Add(new ActivityTag() { Activity = activity, Tag = tag });
            }
        }

        if (input.CategoriesIds != null && input.CategoriesIds.Count > 0)
        {
            activity.ActivityCategories = new List<ActivityCategory>();
            foreach (var category in categories)
            {
                activity.ActivityCategories.Add(new ActivityCategory() { Activity = activity, Category = category});
            }
        }


        _context.Update(activity);
        var result = await _context.SaveChangesAsync() > 0 ? true : false;
        if (!result)
            return new UpdateActivityResult
            {
                Status = UpdateActivityStatus.ErrorWhileUpdating
            };

        return new UpdateActivityResult
        {
            Status = UpdateActivityStatus.Successful
        };
    }
    public async Task<DeleteActivityResult> DeleteActivity(int actId, string username)
    {
        var user = await _userRepository.GetUser(username);
        if (user is null)
            return new DeleteActivityResult
            {
                Status = DeleteActivityStatus.UserDoesntExist
            };

        
        // проверка мероприятия - есть ли в базе?
        var activity = await _context.Activities.FindAsync(actId);
        if (activity is null)
            return new DeleteActivityResult
            {
                Status = DeleteActivityStatus.ActivityDoesntExist
            };

        // есть ли у пользователя доступ к этой группе
        if (activity.Owner.Id != user.Id)
            return new DeleteActivityResult
            {
                Status = DeleteActivityStatus.UserHasNoAccess
            };
        // soft удаление
        activity.Deleted = true;
        _context.Update(activity);

        var result = await _context.SaveChangesAsync() > 0 ? true : false;
        if (!result)
            return new DeleteActivityResult
            {
                Status = DeleteActivityStatus.ErrorWhileDeleting
            };

        return new DeleteActivityResult
        {
            Status = DeleteActivityStatus.Successful
        };
    }

    public async Task<List<ActivityDto>> GetUserActivities(string username)
    {
        var user = await _userRepository.GetUser(username);

        if (user is null)
            return new List<ActivityDto>();

        var activities = await _context
            .Activities
            .Include(a => a.Owner)
            .Include(a => a.City)
            .Include(a => a.Community)
            .Include(a => a.ActivityCategories).ThenInclude(ac => ac.Category)
            .Include(a => a.ActivityTags).ThenInclude(at => at.Tag)
            .Where(a => a.Owner.Id == user.Id)
            .ToListAsync();

        return _mapper.Map<List<ActivityDto>>(activities);
    }
}
