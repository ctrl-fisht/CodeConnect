using CodeConnect.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CodeConnect.Data;

public class AppDbContext : IdentityDbContext<IdentityUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Activity>().HasQueryFilter(a => !a.Deleted);
        modelBuilder.Entity<Community>().HasQueryFilter(c => !c.Deleted);
    }

    public DbSet<User> AppUsers { get; set; } = null!;
    public DbSet<Activity> Activities { get; set; }
    public DbSet<ActivityCategory> ActivityCategories { get; set; }
    public DbSet<ActivityTag> ActivityTags { get; set; }
    public DbSet<ActivityUser> ActivityUsers { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<City> Cities { get; set; }
    public DbSet<Community> Communities { get; set; }
    public DbSet<CommunityUser> CommunityUsers { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<ActivityImage> ActivityImages { get; set; }
    public DbSet<CommunityImage> CommunityImages { get; set; }
    public DbSet<TelegramConnection> TelegramConnections { get; set; }

}
