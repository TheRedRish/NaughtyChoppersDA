using NaughtyChoppersDA.Entities;
using System;

namespace NaughtyChoppersDA.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetUserByUsernameAndPassword(string userName, string password);
        Task CreateUser(string userName, string password);
        Task DeleteUser(User user);
        Task<bool> DoesUserExist(string userName);

        //Task<User> GetUserById(Guid? id);

        //void UpdateUser(User user);
    }
}
