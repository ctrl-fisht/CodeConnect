using CodeConnect.Entities;

namespace CodeConnect.Data;

public class Seed
{
    private readonly AppDbContext _dbContext;

    public Seed(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void SeedDbContext()
    {
        var cityList = new List<City>()
        {
            new City()
            {
                Name = "Красноярск",
                UtcOffsetHours = 7
            },
            new City()
            {
                Name = "Санкт-Петербург",
                UtcOffsetHours = 3
            },
            new City()
            {
                Name = "Москва",
                UtcOffsetHours = 3
            }
        };
        var tagList = new List<Tag>()
        {
            new Tag()
            {
                Title = "C#",
                ColorHex = "#852eff"
            },
            new Tag()
            {
                Title = "Unit-тесты",
                ColorHex = "#3ec966"
            },
            new Tag()
            {
                Title = "Kafka",
                ColorHex = "#97d92e"
            }
        };
        var categoryList = new List<Category>()
        {
            new Category()
            {
                Title = "Менеджмент",
                ColorHex = "#7b19b0"
            },
            new Category()
            {
                Title = "Разработка",
                ColorHex = "#d4a61e"
            },
            new Category()
            {
                Title = "DevOps",
                ColorHex = "#8a2152"
            }
        };


        var admin = _dbContext.AppUsers
            .Where(u => u.UserName == "handakai")
            .First();

        var activityList = new List<Activity>
        {
            new Activity()
            {
                Title = ".NET Meetup",
                DateLocal = DateOnly.FromDateTime(DateTime.Now).AddDays(7),
                TimeLocal = TimeOnly.FromDateTime(DateTime.Now),
                DateUtc = DateOnly.FromDateTime(DateTime.Now).AddDays(7),
                TimeUtc = TimeOnly.FromDateTime(DateTime.Now).AddHours(-7),
                Description = "Митап .NET разработчиков",
                Address = "Ул. Тестовая 24",
                DurationMinutes = 120,
                HasStream = false,
                StreamURL = null,
                IsActive = true,
                TicketPrice = 0,

                Owner = admin,
                Community = new Community()
                {
                    Name = "Комьюнити Красноярск",
                    Description = "Описание будет лишним...",
                    Owner = admin
                }
            }
        };

        _dbContext.AddRange(cityList);
        _dbContext.AddRange(categoryList);
        _dbContext.AddRange(tagList);

        _dbContext.AddRange(activityList);
        _dbContext.SaveChanges();
    }
}
