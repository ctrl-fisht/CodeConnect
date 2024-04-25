using CodeConnect.Entities;

namespace CodeConnect.Users;

public interface IUserRepository
{
    Task<ICollection<User>> GetUsers(int offset, int count);
    Task<User?> GetUserDetailed(string name);

    Task<User?> GetUser(string name);
    Task<User?> GetUserById(string id);

    Task<bool> SetTgId(int tgId, string name);
    Task<bool> RemoveTgId(string name);
         
    Task<bool> UpdateUser(User user);
    Task<bool> Save();
}

