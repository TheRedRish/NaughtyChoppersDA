using NaughtyChoppersDA.Entities;
using System;

namespace NaughtyChoppersDA.Repositories
{
    public interface IUserRepository
    {
        User GetUser(Guid? id);
        User? GetUser(string userName, string password);
        void CreateUser(string userName, string password);
        void DeleteUser(User user);
        void UpdateUser(User user);
        bool DoesUserExist(string userName);
    }
}
