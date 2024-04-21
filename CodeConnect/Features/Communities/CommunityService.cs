﻿using AutoMapper;
using CodeConnect.Dto;
using CodeConnect.Data;
using CodeConnect.Entities;
using CodeConnect.Users;
using Microsoft.EntityFrameworkCore;
using System;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;


namespace CodeConnect.Features.Communities;

public class CommunityService
{
    private readonly IUserRepository _userRepository;
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly IWebHostEnvironment _env;

    public CommunityService(
        AppDbContext context,
        IUserRepository userRepository,
        IMapper mapper,
        IWebHostEnvironment env)
    {
        _context = context;
        _userRepository = userRepository;
        _mapper = mapper;
        _env = env;
    }

    public async Task<CommunityDto?> Get(int commId)
    {
        var community = await _context.Communities
            .Where(c => c.CommunityId == commId)
            .Include(c => c.Image)
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
            .Include(c => c.Owner)
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
            .Include(c => c.Owner)
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

    public async Task<List<CommunityDto>> GetUserCommunities(string username)
    {
        var user = await _userRepository.GetUser(username);

        if (user is null)
            return new List<CommunityDto>();

        var communities = await _context
            .Communities
            .Include(c => c.Owner)
            .Include(c => c.Image)
            .Where(c => c.Owner.Id == user.Id)
            .ToListAsync();

        return _mapper.Map<List<CommunityDto>>(communities);
    }

    public async Task<List<ActivityDto>> GetCommunityActivities(int commId)
    {
        var activities = await _context
            .Activities
            .Include(a => a.Community)
            .Include(a => a.City)
            .Include(a => a.Image)
            .Include(a => a.ActivityTags).ThenInclude(at => at.Tag)
            .Include(a => a.ActivityCategories).ThenInclude(ac => ac.Category)
            .Where(a => a.Community.CommunityId == commId)
            .ToListAsync();

        return _mapper.Map<List<ActivityDto>>(activities);
    }

    public async Task<UpdateCommunityMiniPhotoResult> UpdateMiniPhoto(int commId, IFormFile file, string username)
    {
        var user = await _userRepository.GetUser(username);
        if (user is null)
            return new UpdateCommunityMiniPhotoResult
            {
                Status = UpdateCommunityMiniPhotoStatus.UserDoesntExist
            };

        // проверка сообщества - есть ли в базе?
        var community = await _context.Communities
            .Include(c => c.Owner)
            .Where(c => c.CommunityId == commId)
            .FirstOrDefaultAsync();

        if (community is null)
            return new UpdateCommunityMiniPhotoResult
            {
                Status = UpdateCommunityMiniPhotoStatus.CommunityDoesntExist
            };


        // принадлежит ли комьюнити пользователю
        if (!(user?.Id == community.Owner.Id))
            return new UpdateCommunityMiniPhotoResult
            {
                Status = UpdateCommunityMiniPhotoStatus.UserHasNoAccess
            };

        // проверка РАЗМЕРА ФАЙЛА
        var lengthMb = file.Length / 1024 / 1024;
        if (lengthMb > 5)
            return new UpdateCommunityMiniPhotoResult
            {
                Status = UpdateCommunityMiniPhotoStatus.FileTooBig
            };

        // Проверка - картинка ли это
        if (!file.ContentType.StartsWith("image/"))
            return new UpdateCommunityMiniPhotoResult
            {
                Status = UpdateCommunityMiniPhotoStatus.IncorrectFormat
            };

        var relativePath = $"images\\community\\{commId}";
        var fileName = "mini.jpg";
        var concretePath = Path.Combine(_env.WebRootPath, relativePath);

        if (!Path.Exists(concretePath))
            Directory.CreateDirectory(concretePath);

        var fullPath = Path.Combine(concretePath, fileName);

        using var image = Image.Load(file.OpenReadStream());
        image.Mutate(x => x.Resize(50, 50));
        image.SaveAsJpeg(fullPath);

        var communityImage = await _context
            .CommunityImages
            .Where(ci => ci.CommunityId == community.CommunityId)
            .FirstOrDefaultAsync();

        // путь по которому мы будем получать картинку на фронтенде
        var miniPath = $"/images/community/{commId}/mini.jpg";

        // если не было CommunityImage записи 
        if (communityImage is null)
        {
            communityImage = new CommunityImage() { CommunityId = community.CommunityId };
            communityImage.MiniPath = miniPath;
            _context.CommunityImages.Add(communityImage);
        }
        else
        {
            communityImage.MiniPath = miniPath;
            _context.CommunityImages.Update(communityImage);
        }


        var result = await _context.SaveChangesAsync() > 0 ? true : false;
        if (!result)
            return new UpdateCommunityMiniPhotoResult
            {
                Status = UpdateCommunityMiniPhotoStatus.ErrorWhileUpdating
            };

        return new UpdateCommunityMiniPhotoResult
        {
            Status = UpdateCommunityMiniPhotoStatus.Successful
        };
    }

    public async Task<UpdateCommunitySmallPhotoResult> UpdateSmallPhoto(int commId, IFormFile file, string username)
    {
        var user = await _userRepository.GetUser(username);
        if (user is null)
            return new UpdateCommunitySmallPhotoResult
            {
                Status = UpdateCommunitySmallPhotoStatus.UserDoesntExist
            };

        // проверка сообщества - есть ли в базе?
        var community = await _context.Communities
            .Include(c => c.Owner)
            .Where(c => c.CommunityId == commId)
            .FirstOrDefaultAsync();

        if (community is null)
            return new UpdateCommunitySmallPhotoResult
            {
                Status = UpdateCommunitySmallPhotoStatus.CommunityDoesntExist
            };


        // принадлежит ли комьюнити пользователю
        if (!(user?.Id == community.Owner.Id))
            return new UpdateCommunitySmallPhotoResult
            {
                Status = UpdateCommunitySmallPhotoStatus.UserHasNoAccess
            };

        // проверка РАЗМЕРА ФАЙЛА
        var lengthMb = file.Length / 1024 / 1024;
        if (lengthMb > 5)
            return new UpdateCommunitySmallPhotoResult
            {
                Status = UpdateCommunitySmallPhotoStatus.FileTooBig
            };

        // Проверка - картинка ли это
        if (!file.ContentType.StartsWith("image/"))
            return new UpdateCommunitySmallPhotoResult
            {
                Status = UpdateCommunitySmallPhotoStatus.IncorrectFormat
            };

        var relativePath = $"images\\community\\{commId}";
        var fileName = "small.jpg";
        var concretePath = Path.Combine(_env.WebRootPath, relativePath);

        if (!Path.Exists(concretePath))
            Directory.CreateDirectory(concretePath);

        var fullPath = Path.Combine(concretePath, fileName);

        using var image = Image.Load(file.OpenReadStream());
        image.Mutate(x => x.Resize(144, 144));
        image.SaveAsJpeg(fullPath);

        var communityImage = await _context
            .CommunityImages
            .Where(ci => ci.CommunityId == community.CommunityId)
            .FirstOrDefaultAsync();

        // путь по которому мы будем получать картинку на фронтенде
        var smallPath = $"/images/community/{commId}/{fileName}";

        // если не было CommunityImage записи 
        if (communityImage is null)
        {
            communityImage = new CommunityImage() { CommunityId = community.CommunityId };
            communityImage.SmallPath = smallPath;
            _context.CommunityImages.Add(communityImage);
        }
        else
        {
            communityImage.SmallPath = smallPath;
            _context.CommunityImages.Update(communityImage);
        }


        var result = await _context.SaveChangesAsync() > 0 ? true : false;
        if (!result)
            return new UpdateCommunitySmallPhotoResult
            {
                Status = UpdateCommunitySmallPhotoStatus.ErrorWhileUpdating
            };

        return new UpdateCommunitySmallPhotoResult
        {
            Status = UpdateCommunitySmallPhotoStatus.Successful
        };
    }
}
