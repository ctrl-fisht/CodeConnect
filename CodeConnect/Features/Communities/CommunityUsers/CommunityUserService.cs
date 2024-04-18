using AutoMapper;
using CodeConnect.CommonDto;
using CodeConnect.Data;
using CodeConnect.Entities;
using CodeConnect.Users;
using Microsoft.EntityFrameworkCore;
#pragma warning disable CS8602
#pragma warning disable CS8604

namespace CodeConnect.Features.Communities.CommunityUsers;

public class CommunityUserService
{
    private readonly IUserRepository _userRepository;
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    public CommunityUserService(
        IUserRepository userRepository,
        AppDbContext context,
        IMapper mapper
        )
    {
        _userRepository = userRepository;
        _context = context;
        _mapper = mapper;
    }

    public async Task<CreateCommunityUserResult> Create(int commId, string userName)
    {

        // получаем юзера
        var user = await _userRepository.GetUser(userName);
        if (user is null)
            return new CreateCommunityUserResult
            {
                Status = CreateCommunityUserStatus.UserDoesntExists
            };

        // существует ли комьюнити?
        var community = await _context.Communities.Where(c => c.CommunityId == commId).FirstOrDefaultAsync();
        if (community is null)
            return new CreateCommunityUserResult
            {
                Status = CreateCommunityUserStatus.CommunityDoesntExists
            };


        // уже существует связь CommunityUser ?
        var relation = await _context
            .CommunityUsers
            .Where(cu => cu.UserId == user.Id && cu.CommunityId == commId)
            .FirstOrDefaultAsync();
        if (relation is not null)
            return new CreateCommunityUserResult
            {
                Status = CreateCommunityUserStatus.AlreadyExists
            };

        await _context.AddAsync(new CommunityUser
        {
            CommunityId = commId,
            UserId = user.Id
        });
        var result = await _context.SaveChangesAsync() > 0 ? true : false;

        if (!result)
            return new CreateCommunityUserResult
            {
                Status = CreateCommunityUserStatus.ErrorWhileCreating
            };

        return new CreateCommunityUserResult
        {
            Status = CreateCommunityUserStatus.Successful
        };
    }

    public async Task<DeleteCommunityUserResult> Delete(int commId, string userName)
    {
        // получаем юзера
        var user = await _userRepository.GetUser(userName);
        if (user is null)
            return new DeleteCommunityUserResult
            {
                Status = DeleteCommunityUserStatus.UserDoesntExists
            };

        // существует ли комьюнити?
        var community = await _context.Communities.Where(c => c.CommunityId == commId).FirstOrDefaultAsync();
        if (community is null)
            return new DeleteCommunityUserResult
            {
                Status = DeleteCommunityUserStatus.CommunityDoesntExists
            };


        // подписан ли я
        var relation = await _context
            .CommunityUsers
            .Where(cu => cu.UserId == user.Id && cu.CommunityId == commId)
            .FirstOrDefaultAsync();
        if (relation is null)
            return new DeleteCommunityUserResult
            {
                Status = DeleteCommunityUserStatus.DoesntExists
            };
        _context.Remove(relation);

        var result = await _context.SaveChangesAsync() > 0 ? true : false;

        if (!result)
            return new DeleteCommunityUserResult
            {
                Status = DeleteCommunityUserStatus.ErrorWhileDeleting
            };

        return new DeleteCommunityUserResult
        {
            Status = DeleteCommunityUserStatus.Successful
        };
    }

    public async Task<List<CommunityDto>> GetSubscribedCommunities(string userName)
    {
        var user = await _userRepository.GetUser(userName);

        if (user is null)
            return new List<CommunityDto>();

        var communities = await _context
            .CommunityUsers
            .Include(cu => cu.Community)
            .Where(cu => cu.UserId == user.Id)
            .Select(cu => cu.Community)
            .ToListAsync();

        return _mapper.Map<List<CommunityDto>>(communities);
    }

    public async Task<bool> IsSubscriber(string userName, int commId)
    {
        var user = await _userRepository.GetUser(userName);

        if (user is null)
            return false;

        return await _context.CommunityUsers.AnyAsync(cu => cu.UserId == user.Id && cu.CommunityId == commId);

    }
}
#pragma warning restore CS8602
#pragma warning disable CS8604
