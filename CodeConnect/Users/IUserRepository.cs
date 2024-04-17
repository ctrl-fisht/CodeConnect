using CodeConnect.Entities;

namespace CodeConnect.Users;

public interface IUserRepository
{
    Task<ICollection<User>> GetUsers(int offset, int count);
    Task<User?> GetUserDetailed(string name);

    Task<User?> GetUser(string name);
    Task<bool> UpdateUser(User user);
    Task<bool> Save();
}

