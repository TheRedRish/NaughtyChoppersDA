using NaughtyChoppersDA.Entities;
using System.Diagnostics.Eventing.Reader;

namespace NaughtyChoppersDA.Services
{
    public interface IUserService
    {
        User? User { get; set; }
        Task<string> CreateUser(string userName, string password);
        Task DeleteUser(User user);
        Task<string> Login(string userName, string password);
        void LogOut();
        Task<bool> DoesUserExist(string userName);

        //Task<bool> UpdateUser(User user);
    }
}
