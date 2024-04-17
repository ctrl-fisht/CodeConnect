using CodeConnect.Data;
using CodeConnect.Entities;

namespace CodeConnect.Features.CommunityUsers;

public class CommunityUserRepository
{
    private readonly AppDbContext _context;
    public CommunityUserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Create(CommunityUser communityUser)
    {
        await _context.AddAsync(communityUser);
        return await Save();
    }


    public async Task<bool> Save()
    {
        return await _context.SaveChangesAsync() > 0 ? true : false;
    }
}
