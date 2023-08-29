using Microsoft.AspNetCore.Components;
using NaughtyChoppersDA.Entities;
using NaughtyChoppersDA.Globals;
using NaughtyChoppersDA.Repositories;


namespace NaughtyChoppersDA.Services
{
    public class UserService : IUserService
    {
        private IUserRepository _repository;

        public UserService(IUserRepository userRepository)
        {
            _repository = userRepository;
        }

        public User? User { get; set; }

        public async Task<string> CreateUser(string userName, string password)
        {
            try
            {
                await _repository.CreateUser(userName, password);
                return "Success";
            }
            catch (UserException ex)
            {
                return ex.Message;
            }
        }

        public async Task DeleteUser(User user)
        {
            try
            {
                await _repository.DeleteUser(user);
            }
            catch (UserException ex)
            {
                throw ex;
            }
        }

        public async Task<string> Login(string userName, string password)
        {
            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password)) 
            {
                return "Empty field";
            }
            try
            {
                User = await _repository.GetUserByUsernameAndPassword(userName, password);

                if (User == null)
                {
                    return "User not found";
                }
                return "Success";
            }
            catch(UserException ex)
            {
                return ex.Message;
            }


        }

        public void LogOut()
        {
            User = null;
        }


        public async Task<bool> DoesUserExist(string userName)
        {
            try
            {
                 return await _repository.DoesUserExist(userName);
            }
            catch (UserException ex)
            {
                throw ex;
            }
        }

        //public bool UpdateUser(User user)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
