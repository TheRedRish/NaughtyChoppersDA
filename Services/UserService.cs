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

        public string CreateUser(string userName, string password)
        {
            try
            {
                _repository.CreateUser(userName, password);
                return "Success";
            }
            catch (UserException ex)
            {
                return ex.Message;
            }
        }

        public void DeleteUser(User user)
        {
            try
            {
                _repository.DeleteUser(user);
            }
            catch (UserException ex)
            {
                throw ex;
            }
        }

        public string Login(string userName, string password)
        {
            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password)) 
            {
                return "Empty field";
            }
            try
            {
                User = _repository.GetUser(userName, password);

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

        public bool UpdateUser(User user)
        {
            throw new NotImplementedException();
        }

        public bool DoesUserExist(string userName)
        {
            try
            {
                return _repository.DoesUserExist(userName);
            }
            catch (UserException ex)
            {
                throw ex;
            }
        }
    }
}
