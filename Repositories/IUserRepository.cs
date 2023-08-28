using NaughtyChoppersDA.Entities;
using System;

namespace NaughtyChoppersDA.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetUser(string userName, string password);
        void CreateUser(string userName, string password);
        Task DeleteUser(User user);
        Task<bool> DoesUserExist(string userName);

        //Task<User> GetUserById(Guid? id);

        //void UpdateUser(User user);
    }
}
