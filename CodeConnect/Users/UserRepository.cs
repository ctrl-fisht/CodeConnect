﻿using CodeConnect.Data;
using CodeConnect.Entities;
using Microsoft.EntityFrameworkCore;

namespace CodeConnect.Users;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> UpdateUser(User user)
    {
        _context
            .Update(user);
        return await Save();
    }

    // todo: добавить то что нужно.
    public async Task<User?> GetUserDetailed(string name)
    {
        return await _context.AppUsers
            .Where(u => u.UserName == name)
            .Include(u => u.City)
            .FirstOrDefaultAsync();
    }

    public async Task<ICollection<User>> GetUsers(int offset, int count)
    {
        return await _context.AppUsers
            .Skip(offset)
            .Take(count)
            .OrderBy(u => u.Id)
            .ToListAsync();
    }

    public async Task<bool> Save()
    {
        return await _context.SaveChangesAsync() > 0 ? true : false;
    }

    public async Task<User?> GetUser(string userName)
    {
        return await _context.AppUsers
            .Where(u => u.UserName == userName)
            .Include(u => u.City)
            .FirstOrDefaultAsync();
    }

    public async Task<bool> SetTgId(int tgId, string name)
    {
        var user = await GetUser(name);

        if (user is null)
            return false;

        user.TgUserId = tgId;
        _context.Update(user);

        return await Save();
    }

    public async Task<bool> RemoveTgId(string name)
    {
        var user = await GetUser(name);

        if (user is null)
            return false;

        user.TgUserId = null;
        user.EnableTgNotif = false;

        _context.Update(user);

        return await Save();
    }

    public async Task<User?> GetUserById(string id)
    {
        return await _context.AppUsers
            .Where(u => u.Id == id)
            .Include(u => u.City)
            .FirstOrDefaultAsync();
    }
}

