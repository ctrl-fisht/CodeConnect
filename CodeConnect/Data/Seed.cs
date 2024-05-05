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
            },
            new City()
            {
                Name = "Нижний новгород",
                UtcOffsetHours = 3
            },
            new City()
            {
                Name = "Новосибирск",
                UtcOffsetHours = 7
            },

            new City()
            {
                Name = "Екатеринбург",
                UtcOffsetHours = 5
            },

            new City()
            {
                Name = "Казань",
                UtcOffsetHours = 3
            },

            new City()
            {
                Name = "Челябинск",
                UtcOffsetHours = 5
            },

            new City()
            {
                Name = "Самара",
                UtcOffsetHours = 4
            },

            new City()
            {
                Name = "Уфа",
                UtcOffsetHours = 5
            },

            new City()
            {
                Name = "Ростов-на-Дону",
                UtcOffsetHours = 3
            },

            new City()
            {
                Name = "Краснодар",
                UtcOffsetHours = 3
            },

            new City()
            {
                Name = "Омск",
                UtcOffsetHours = 6
            },

            new City()
            {
                Name = "Воронеж",
                UtcOffsetHours = 3
            },
            new City()
            {
                Name = "Пермь",
                UtcOffsetHours = 5
            },
            new City()
            {
                Name = "Волгоград",
                UtcOffsetHours = 3
            },
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
            },
            new Tag()
            {
                Title = "Javascript",
                ColorHex ="#ffc72b"
            },
            new Tag()
            {
                Title = "Android",
                ColorHex ="#9ae637"
            },
            new Tag()
            {
                Title = "Go",
                ColorHex ="#2db5b1"
            },
            new Tag()
            {
                Title = "Java",
                ColorHex ="#0eebe4"
            },
            new Tag()
            {
                Title = "Python",
                ColorHex ="#1177bf"
            },
            new Tag()
            {
                Title = "Tinkoff",
                ColorHex ="#bc6dfc"
            },
            new Tag()
            {
                Title = "Rust",
                ColorHex ="#f558cb"
            },
            new Tag()
            {
                Title = "Avito",
                ColorHex ="#e05877"
            },
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
                Title = "DevOps/Cloud",
                ColorHex = "#8a2152"
            },
            new Category()
            {
                Title = "DevRel",
                ColorHex = "#f27491"
            },
            new Category()
            {
                Title = "Data Science / Big Data",
                ColorHex = "#ff00c3"
            },
            new Category()
            {
                Title = "Продакт-менеджмент",
                ColorHex = "#cc274d"
            },
            new Category()
            {
                Title = "ML/AI",
                ColorHex = "#5cd1e0"
            },
            new Category()
            {
                Title = "Бэкенд",
                ColorHex = "#62e382"
            },
            new Category()
            {
                Title = "Мобильная разработка",
                ColorHex = "#dce060"
            },
            new Category()
            {
                Title = "BI",
                ColorHex = "#81e833"
            },
            new Category()
            {
                Title = "Дизайн UI/UX",
                ColorHex = "#33afe8"
            },
            new Category()
            {
                Title = "Soft Skills",
                ColorHex = "#8d5bf0"
            },
            new Category()
            {
                Title = "Безопасность",
                ColorHex = "#cc394a"
            },
            new Category()
            {
                Title = "Тестирование",
                ColorHex = "#a8275d"
            },
            new Category()
            {
                Title = "IoT",
                ColorHex = "#395bd4"

            },
            new Category()
            {
                Title = "Фронтенд",
                ColorHex = "#1ac450"
            },
            new Category()
            {
                Title = "HR",
                ColorHex = "#dbd407"
            },
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

                City = cityList[0],
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
