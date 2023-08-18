using NaughtyChoppersDA.Entities;
using System.Diagnostics.Eventing.Reader;

namespace NaughtyChoppersDA.Services
{
    public interface IUserService
    {
        User? User { get; set; }
        string CreateUser(string userName, string password);
        void DeleteUser(User user);
        bool UpdateUser(User user);
        string Login(string userName, string password);
        void LogOut();
        bool DoesUserExist(string userName);
    }
}
