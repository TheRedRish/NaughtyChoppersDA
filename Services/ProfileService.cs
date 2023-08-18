using NaughtyChoppersDA.Entities;

namespace NaughtyChoppersDA.Services
{
    public class ProfileService : IProfileService
    {
        public Profile? Profile { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public bool CreateProfile(string userName, string password)
        {
            throw new NotImplementedException();
        }

        public string CreateProfile(Profile profile)
        {
            throw new NotImplementedException();
        }

        public string DeleteProfile(Guid id)
        {
            throw new NotImplementedException();
        }

        public User GetProfile(Guid id)
        {
            throw new NotImplementedException();
        }

        public string UpdateProfile(Profile profile)
        {
            throw new NotImplementedException();
        }
    }
}
