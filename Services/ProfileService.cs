using NaughtyChoppersDA.Entities;
using NaughtyChoppersDA.Repositories;

namespace NaughtyChoppersDA.Services
{
    public class ProfileService : IProfileService
    {
        private IProfileRepository _repository;
        public ProfileService(IProfileRepository profileRepository)
        {
            _repository = profileRepository;
        }
        public Profile? Profile { get; set; }

        public bool CreateProfile(string userName, string password)
        {
            throw new NotImplementedException();
        }

        public string CreateProfile(Profile profile, User user)
        {
            _repository.CreateProfile(profile, user);
            return "Succes";
        }

        public string DeleteProfile(Guid id)
        {
            throw new NotImplementedException();
        }

        public List<HelicopterModel> GetAllHelicopterModels()
        {
            return _repository.GetAllHelicoptersModels();
        }

        public string? GetCityByPostalCode(string postalCode)
        {
            return _repository.GetCityByPostalCode(postalCode);
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
