using AutoMapper;
using CodeConnect.Dto;
using CodeConnect.Data;
using CodeConnect.Entities;
using CodeConnect.Users;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace CodeConnect.Features.Activities;

public class ActivityService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _env;
    
    public ActivityService(IUserRepository userRepository, IMapper mapper, AppDbContext context, IWebHostEnvironment env)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _context = context;
        _env = env;
    }

    public async Task<ActivityDto?> Get(int actId)
    {


        var activity = await _context
            .Activities
            .Include(a => a.City)
            .Include(a => a.ActivityCategories).ThenInclude(ac => ac.Category)
            .Include(a => a.ActivityTags).ThenInclude(at => at.Tag)
            .Include(a => a.Community).ThenInclude(c => c.Image)
            .Include(a => a.Image)
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
        
        newActivity.IsActive = false;
        newActivity.Declined = false;
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
        var dateTime = DateTime.UtcNow;
        var date = DateOnly.FromDateTime(dateTime);
        var time = TimeOnly.FromDateTime(dateTime);


        var activities = await _context
            .Activities
            .Include(a => a.City)
            .Include(a => a.Community)
            .Include(a => a.ActivityCategories).ThenInclude(ac => ac.Category)
            .Include(a => a.ActivityTags).ThenInclude(at => at.Tag)
            .Include(a => a.Image)
            .Where(a => a.DateUtc > date || (a.DateUtc == date && a.TimeUtc > time))
            .OrderBy(a => a.DateUtc)
            .ThenBy(a => a.TimeUtc)
            .Skip(offset)
            .Take(count)
            .ToListAsync();

        return _mapper.Map<List<ActivityDto>>(activities);


    }

    public async Task<List<ActivityDto>> GetActivityListPast(int offset, int count)
    {
        var dateTime = DateTime.UtcNow;
        var date = DateOnly.FromDateTime(dateTime);
        var time = TimeOnly.FromDateTime(dateTime);

        var activities = await _context
            .Activities
            .Skip(offset)
            .Take(count)
            .Include(a => a.City)
            .Include(a => a.Community)
            .Include(a => a.ActivityCategories).ThenInclude(ac => ac.Category)
            .Include(a => a.ActivityTags).ThenInclude(at => at.Tag)
            .Include(a => a.Image)
            .Where(a => a.DateUtc < date || (a.DateUtc == date && a.TimeUtc < time))
            .OrderByDescending(a => a.DateUtc)
            .ThenByDescending(a => a.TimeUtc)
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
            .IgnoreQueryFilters()
            .Where(a => a.Deleted != true && a.Declined != true)
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
        if (input.TagsIds != null)
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
        if (input.CategoriesIds != null)
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


        if (input.DateLocal != null && input.TimeLocal != null)
        {
            // считаем из локального времени + часового пояса города в котором мероприятие - UTC время и дату
            var utcDateAndTime = TimeConverter.CalculateUtcFromLocal(input.DateLocal.Value, input.TimeLocal.Value, city.UtcOffsetHours);

            activity.DateUtc = utcDateAndTime.Item1;
            activity.DateLocal = (DateOnly)input.DateLocal;

            activity.TimeLocal = (TimeOnly)input.TimeLocal;
            activity.TimeUtc = utcDateAndTime.Item2;
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
        
        if (input.TagsIds != null)
        {
            activity.ActivityTags = new List<ActivityTag>();
            foreach (var tag in tags)
            {
                activity.ActivityTags.Add(new ActivityTag() { Activity = activity, Tag = tag });
            }
        }

        if (input.CategoriesIds != null)
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

    public async Task<UpdateActivitySmallPhotoResult> UpdateSmallPhoto(int actId, IFormFile file, string username)
    {
        var user = await _userRepository.GetUser(username);
        if (user is null)
            return new UpdateActivitySmallPhotoResult
            {
                Status = UpdateActivitySmallPhotoStatus.UserDoesntExist
            };

        // проверка мероприятия - есть ли в базе?
        var activity = await _context.Activities
            .IgnoreQueryFilters()
            .Where(a => a.Deleted != true && a.Declined != true)
            .Where(a => a.ActivityId == actId)
            .FirstOrDefaultAsync();

        if (activity is null)
            return new UpdateActivitySmallPhotoResult
            {
                Status = UpdateActivitySmallPhotoStatus.ActivityDoesntExist
            };

        // есть ли у пользователя доступ к этой группе
        if (activity.Owner.Id != user.Id)
            return new UpdateActivitySmallPhotoResult
            {
                Status = UpdateActivitySmallPhotoStatus.UserHasNoAccess
            };

        // проверка РАЗМЕРА ФАЙЛА
        var lengthMb = file.Length / 1024 / 1024;
        if (lengthMb > 5)
            return new UpdateActivitySmallPhotoResult
            {
                Status = UpdateActivitySmallPhotoStatus.FileTooBig
            };

        // Проверка - картинка ли это
        if (!file.ContentType.StartsWith("image/"))
            return new UpdateActivitySmallPhotoResult
            {
                Status = UpdateActivitySmallPhotoStatus.IncorrectFormat
            };

        var relativePath = $"images/activity/{actId}";
        var fileName = "small.jpg";
        var concretePath = Path.Combine(_env.WebRootPath, relativePath);

        if (!Path.Exists(concretePath))
            Directory.CreateDirectory(concretePath);

        var fullPath = Path.Combine(concretePath, fileName);

        using var image = Image.Load(file.OpenReadStream());
        image.Mutate(x => x.Resize(288, 288));
        image.SaveAsJpeg(fullPath);

        var activityImage = await _context
            .ActivityImages
            .Where(ai => ai.ActivityId == activity.ActivityId)
            .FirstOrDefaultAsync();

        // путь по которому мы будем получать картинку на фронтенде
        var smallPath = $"/images/activity/{actId}/small.jpg";

        // если не было ActivityImage записи 
        if (activityImage is null)
        {
            activityImage = new ActivityImage() { ActivityId = activity.ActivityId };
            activityImage.SmallPath = smallPath;
            _context.ActivityImages.Add(activityImage);
        }
        else
        {
            activityImage.SmallPath = smallPath;
            _context.ActivityImages.Update(activityImage);
        }

        
        var result = await _context.SaveChangesAsync() > 0 ? true : false;
        if (!result)
            return new UpdateActivitySmallPhotoResult
            {
                Status = UpdateActivitySmallPhotoStatus.ErrorWhileUpdating
            };

        return new UpdateActivitySmallPhotoResult
        {
            Status = UpdateActivitySmallPhotoStatus.Successful
        };
    }

    public async Task<UpdateActivityBannerPhotoResult> UpdateBannerPhoto(int actId, IFormFile file, string username)
    {
        var user = await _userRepository.GetUser(username);
        if (user is null)
            return new UpdateActivityBannerPhotoResult
            {
                Status = UpdateActivityBannerPhotoStatus.UserDoesntExist
            };

        // проверка мероприятия - есть ли в базе?
        var activity = await _context.Activities
            .IgnoreQueryFilters()
            .Where(a => a.Deleted != true && a.Declined != true)
            .Where(a => a.ActivityId == actId)
            .FirstOrDefaultAsync();

        if (activity is null)
            return new UpdateActivityBannerPhotoResult
            {
                Status = UpdateActivityBannerPhotoStatus.ActivityDoesntExist
            };

        // есть ли у пользователя доступ к этой группе
        if (activity.Owner.Id != user.Id)
            return new UpdateActivityBannerPhotoResult
            {
                Status = UpdateActivityBannerPhotoStatus.UserHasNoAccess
            };

        // проверка РАЗМЕРА ФАЙЛА
        var lengthMb = file.Length / 1024 / 1024;
        if (lengthMb > 5)
            return new UpdateActivityBannerPhotoResult
            {
                Status = UpdateActivityBannerPhotoStatus.FileTooBig
            };

        // Проверка - картинка ли это
        if (!file.ContentType.StartsWith("image/"))
            return new UpdateActivityBannerPhotoResult
            {
                Status = UpdateActivityBannerPhotoStatus.IncorrectFormat
            };

        var relativePath = $"images/activity/{actId}";
        var fileName = "banner.jpg";
        var concretePath = Path.Combine(_env.WebRootPath, relativePath);

        if (!Path.Exists(concretePath))
            Directory.CreateDirectory(concretePath);

        var fullPath = Path.Combine(concretePath, fileName);

        using var image = Image.Load(file.OpenReadStream());
        //image.Mutate(x => x.Resize(1536, 384));
        image.SaveAsJpeg(fullPath);

        var activityImage = await _context
            .ActivityImages
            .Where(ai => ai.ActivityId == activity.ActivityId)
            .FirstOrDefaultAsync();

        // путь по которому мы будем получать картинку на фронтенде
        var bannerPath = $"/images/activity/{actId}/banner.jpg";

        // если не было ActivityImage записи 
        if (activityImage is null)
        {
            activityImage = new ActivityImage() { ActivityId = activity.ActivityId };
            activityImage.BannerPath = bannerPath;
            _context.ActivityImages.Add(activityImage);
        }
        else
        {
            activityImage.BannerPath = bannerPath;
            _context.ActivityImages.Update(activityImage);
        }


        var result = await _context.SaveChangesAsync() > 0 ? true : false;
        if (!result)
            return new UpdateActivityBannerPhotoResult
            {
                Status = UpdateActivityBannerPhotoStatus.ErrorWhileUpdating
            };

        return new UpdateActivityBannerPhotoResult
        {
            Status = UpdateActivityBannerPhotoStatus.Successful
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
        var activity = await _context.Activities
            .IgnoreQueryFilters()
            .Where(a => a.Deleted != true && a.Declined != true)
            .Where(a => a.ActivityId == actId)
            .FirstOrDefaultAsync();
        if (activity is null)
            return new DeleteActivityResult
            {
                Status = DeleteActivityStatus.ActivityDoesntExist
            };

        // есть ли у пользователя доступ к этому мероприятию
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

    public async Task<List<ActivityDto>> GetUserActivities(string username, bool past)
    {
        var user = await _userRepository.GetUser(username);

        if (user is null)
            return new List<ActivityDto>();

        var dateTime = DateTime.UtcNow;
        var date = DateOnly.FromDateTime(dateTime);
        var time = TimeOnly.FromDateTime(dateTime);

        List<Activity> activities = new List<Activity>();

        if (past)
            activities = await _context
            .Activities
            .Include(a => a.Owner)
            .Include(a => a.City)
            .Include(a => a.Community)
            .Include(a => a.ActivityCategories).ThenInclude(ac => ac.Category)
            .Include(a => a.ActivityTags).ThenInclude(at => at.Tag)
            .Include(a => a.Image)
            .Where(a => a.Owner.Id == user.Id)
            .Where(a => a.DateUtc < date || (a.DateUtc == date && a.TimeUtc < time))
            .OrderByDescending(a => a.DateUtc)
            .ThenByDescending(a => a.TimeUtc)
            .ToListAsync();
        else
        activities = await _context
            .Activities
            .Include(a => a.Owner)
            .Include(a => a.City)
            .Include(a => a.Community)
            .Include(a => a.ActivityCategories).ThenInclude(ac => ac.Category)
            .Include(a => a.ActivityTags).ThenInclude(at => at.Tag)
            .Include(a => a.Image)
            .Where(a => a.Owner.Id == user.Id)
            .Where(a => a.DateUtc > date || (a.DateUtc == date && a.TimeUtc > time))
            .OrderBy(a => a.DateUtc)
            .ThenBy(a => a.TimeUtc)
            .ToListAsync();

        return _mapper.Map<List<ActivityDto>>(activities);
    }

    public async Task<bool> IsUserActivity(string username, int actId)
    {
        var user = await _userRepository.GetUser(username);

        if (user is null)
            return false;

        return _context.Activities.IgnoreQueryFilters().Where(a => a.Owner.Id == user.Id && a.ActivityId == actId && a.Deleted == false).Any();
    }

    public async Task<List<ActivityDto>> GetUserModerationActivities(string username)
    {
        var user = await _userRepository.GetUser(username);

        if (user is null)
            return new List<ActivityDto>();

        var activities = await _context
            .Activities
            .IgnoreQueryFilters()
            .Include(a => a.Owner)
            .Include(a => a.City)
            .Include(a => a.Community)
            .Include(a => a.ActivityCategories).ThenInclude(ac => ac.Category)
            .Include(a => a.ActivityTags).ThenInclude(at => at.Tag)
            .Include(a => a.Image)
            .Where(a => a.Owner.Id == user.Id)
            .Where(a => a.IsActive == false && a.Declined == false && a.Deleted == false)
            .OrderByDescending(a => a.DateUtc)
            .ThenByDescending(a => a.TimeUtc)
            .ToListAsync();

        return _mapper.Map<List<ActivityDto>>(activities);
    }

    public async Task<ActivityDto?> GetPreview(string username, int actId, bool admin = false)
    {
        var user = await _userRepository.GetUser(username);

        Activity? activity;

        if (user is null)
            return null;
        if (admin == false)
            activity = await _context
                .Activities
                .IgnoreQueryFilters()
                .Include(a => a.Owner)
                .Include(a => a.City)
                .Include(a => a.ActivityCategories).ThenInclude(ac => ac.Category)
                .Include(a => a.ActivityTags).ThenInclude(at => at.Tag)
                .Include(a => a.Community).ThenInclude(c => c.Image)
                .Include(a => a.Image)
                .Where(a => a.Deleted != true && a.Declined != true)
                .Where(a => a.ActivityId == actId)
                .Where(a => a.Owner.Id == user.Id)
                .FirstOrDefaultAsync();
        else
            activity = await _context
                .Activities
                .IgnoreQueryFilters()
                .Include(a => a.Owner)
                .Include(a => a.City)
                .Include(a => a.ActivityCategories).ThenInclude(ac => ac.Category)
                .Include(a => a.ActivityTags).ThenInclude(at => at.Tag)
                .Include(a => a.Community).ThenInclude(c => c.Image)
                .Include(a => a.Image)
                .Where(a => a.Deleted != true && a.Declined != true)
                .Where(a => a.ActivityId == actId)
                .FirstOrDefaultAsync();

        return _mapper.Map<ActivityDto>(activity);
    }
}
