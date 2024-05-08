using CodeConnect.Data;
using CodeConnect.Features.Activities;
using CodeConnect.Features.Activities.ActivityUsers;
using CodeConnect.Features.Activities.Search;
using CodeConnect.Features.Admin;
using CodeConnect.Features.Auth;
using CodeConnect.Features.Caterories;
using CodeConnect.Features.Cities;
using CodeConnect.Features.Communities;
using CodeConnect.Features.Communities.CommunityUsers;
using CodeConnect.Features.Notifications;
using CodeConnect.Features.Notifications.Scheduling;
using CodeConnect.Features.Tags;
using CodeConnect.Features.Telegram;
using CodeConnect.TelegramBot;
using CodeConnect.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// add user repository (DRY reasons)
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Add services
builder.Services.AddScoped<CommunityService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<CommunityUserService>();
builder.Services.AddScoped<ActivityUserService>();
builder.Services.AddScoped<ActivityService>();
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<TagService>();
builder.Services.AddScoped<CityService>();
builder.Services.AddScoped<SearchService>();
builder.Services.AddScoped<TelegramService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<AdminService>();

// add hosted services
builder.Services.AddHostedService<TelegramBotService>();


// add auto mapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());



builder.Services.AddControllers().AddJsonOptions(x =>
                 x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles); ;

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// add database driver
ConfigurationManager configuration = builder.Configuration;

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

// add identity 
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// add custom quartz scheduling (features/notification/QuartzDependencyInjection.cs)
builder.Services.AddTgNotification();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidAudience = configuration["JWT:Audience"],
            ValidIssuer = configuration["JWT:Issuer"],
            #pragma warning disable CS8604
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]))
            #pragma warning restore CS8604

        };
    });

// add CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.WithOrigins("http://localhost:8080/", "http://localhost:8081/", "http://localhost:8080")
            .AllowAnyMethod()
            .AllowAnyHeader();
        });
});

// add seed service
if (args.Length == 1 && args[0].ToLower() == "seeddata")
    builder.Services.AddScoped<Seed>();

// add swagger in develompment environment
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddSwaggerGen(opt =>
    {
        opt.SwaggerDoc("v1", new OpenApiInfo { Title = "MyAPI", Version = "v1" });

        opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter token",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = "bearer"
        });

        opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[]{ }
        }
    });
    });
}


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = context =>
    {
        if (context.File.Name.EndsWith(".jpg") || context.File.Name.EndsWith(".png"))
        {
            context.Context.Response.Headers.Append("Cache-Control", "no-store, no-cache");
        }
    }
});

app.UseAuthorization();

app.MapControllers();


if (args.Length == 1 && args[0].ToLower() == "seeddata")
{
    SeedData(app);
    return;
}
    


app.Run();


void SeedData(IHost app)
{
#pragma warning disable CS8604
#pragma warning disable CS8602
    var scopedFactory = app.Services.GetService<IServiceScopeFactory>();

    using (var scope = scopedFactory.CreateScope())
    {
        var services = scope.ServiceProvider;
        var seed = scope.ServiceProvider.GetService<Seed>();
        UserRoleInitializer.InitializeAsync(services).Wait();
        seed.SeedDbContext();
    }
#pragma warning restore CS8602
#pragma warning restore CS8604
}