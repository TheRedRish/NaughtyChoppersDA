using NaughtyChoppersDA.Entities;
using NaughtyChoppersDA.Repositories;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

        public string CreateProfile(Profile profile, User user)
        {
            _repository.CreateProfile(profile, user);
            return "Succes";
        }

        public string DeleteProfile(Guid profileId)
        {
            _repository.DeleteProfile(profileId);
            return "Succes";
        }

        public List<HelicopterModel> GetAllHelicopterModels()
        {
            return _repository.GetAllHelicoptersModels();
        }

        public List<HobbyInterest> GetAllHobbyInterests()
        {
            return _repository.GetAllHobbyInterests();            
        }

        public string? GetCityByPostalCode(string postalCode)
        {
            return _repository.GetCityByPostalCode(postalCode);
        }

        public Profile GetProfile(Guid userId)
        {
            return _repository.GetProfileByUserId(userId);
        }

        public string UpdateProfile(Profile profile)
        {
            throw new NotImplementedException();
        }
    }
}
