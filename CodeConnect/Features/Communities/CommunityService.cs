using AutoMapper;
using CodeConnect.CommonDto;
using CodeConnect.Data;
using CodeConnect.Entities;
using CodeConnect.Users;
using Microsoft.EntityFrameworkCore;


namespace CodeConnect.Features.Communities;

public class CommunityService
{
    private readonly IUserRepository _userRepository;
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public CommunityService(
        AppDbContext context,
        IUserRepository userRepository,
        IMapper mapper)
    {
        _context = context;
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<CommunityDto?> Get(int commId)
    {
        var community = await _context.Communities
            .Where(c => c.CommunityId == commId)
            .FirstOrDefaultAsync();

        if (community is null)
            return null;

        return _mapper.Map<CommunityDto>(community);
        
    }

    public async Task<CreateCommunityResult> Create(CreateCommunityInput input, string userName)
    {
        var newCommunity = _mapper.Map<Community>(input);

        //// существует ли пользователь
        var user = await _userRepository.GetUser(userName);
        if (user is null)
            return new CreateCommunityResult
            {
                Status = CreateCommunityStatus.UserDoesntExists
            };

        // существует ли комьюнити с таким именем
        var exist = await _context.Communities
            .AnyAsync(c => c.Name.ToLower() == input.Name.ToLower());

        if (exist)
            return new CreateCommunityResult
            {
                Status = CreateCommunityStatus.AlreadyExists
            };


        newCommunity.Owner = user;

        await _context.AddAsync(newCommunity);
        var result = await _context.SaveChangesAsync() > 0 ? true : false;

        if (!result)
            return new CreateCommunityResult
            {
                Status = CreateCommunityStatus.AlreadyExists
            };

        return new CreateCommunityResult
        {
            Status = CreateCommunityStatus.Successful
        };
    }


    public async Task<DeleteCommunityResult> Delete(int commId, string userName)
    {
        var user = await _userRepository.GetUser(userName);
        if (user is null)
            return new DeleteCommunityResult
            {
                Status = DeleteCommunityStatus.Successful
            };

        var community = await _context.Communities
            .Where(c => c.CommunityId == commId)
            .FirstOrDefaultAsync();

        if (community is null)
            return new DeleteCommunityResult
            {
                Status = DeleteCommunityStatus.CommunityDoesntExists
            };


        // does user own group
        if (!(user.Id == community.Owner.Id))
            return new DeleteCommunityResult
            {
                Status = DeleteCommunityStatus.UserHasNoAccess
            };
        community.Deleted = true;
        _context.Update(community);
        var result = await _context.SaveChangesAsync() > 0 ? true : false;

        if (!result)
            return new DeleteCommunityResult
            {
                Status = DeleteCommunityStatus.ErrorWhileDeleting
            };

        return
        new DeleteCommunityResult
        {
            Status = DeleteCommunityStatus.Successful
        };

    }

    public async Task<UpdateCommunityResult> Update(int commId, UpdateCommunityInput input, string userName)
    {
        var user = await _userRepository.GetUser(userName);

        if (user is null)
            return new UpdateCommunityResult
            {
                Status = UpdateCommunityStatus.UserDoesntExists
            };

        var community = await _context.Communities
            .Where(c => c.CommunityId == commId)
            .FirstOrDefaultAsync();

        if (community is null)
            return new UpdateCommunityResult
            {
                Status = UpdateCommunityStatus.CommunityDoesntExists
            };


        // принадлежит ли комьюнити пользователю
        if (!(user?.Id == community.Owner.Id))
            return new UpdateCommunityResult
            {
                Status = UpdateCommunityStatus.UserHasNoAccess
            };

        if (input.Description != null)
            community.Description = input.Description;
        
        if (input.Name != null)
            community.Name = input.Name;

        _context.Update(community);
        var result = await _context.SaveChangesAsync() > 0 ? true : false;

        if (!result)
            return new UpdateCommunityResult
            {
                Status = UpdateCommunityStatus.ErrorWhileUpdating
            };

        return new UpdateCommunityResult
        {
            Status = UpdateCommunityStatus.Successful
        };
    }
}
