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

        public event EventHandler ProfileUpdated;

        private Profile? _profile;

        public Profile? Profile
        {
            get => _profile;
            set
            {
                _profile = value;
                ProfileUpdated?.Invoke(this, EventArgs.Empty);
            }
        }

        public async Task<string> CreateProfile(Profile profile, User user)
        {
            await _repository.CreateProfile(profile, user);
            return "Succes";
        }

        public async Task<string> DeleteProfile(Guid profileId)
        {
            await _repository.DeleteProfile(profileId);
            return "Succes";
        }

        public async Task<List<HelicopterModel>> GetAllHelicopterModels()
        {
            return await _repository.GetAllHelicoptersModels();
        }

        public async Task<List<HobbyInterest>> GetAllHobbyInterests()
        {
            return await _repository.GetAllHobbyInterests();            
        }

        public async Task<string?> GetCityByPostalCode(string postalCode)
        {
            return await _repository.GetCityByPostalCode(postalCode);
        }

        public async Task<Profile> GetProfileByProfileId(Guid profileId)
        {
            return await _repository.GetProfileByProfileId(profileId);
        }

        public async Task<Profile?> GetProfileByUserId(Guid userId)
        {
            return await _repository.GetProfileByUserId(userId);
        }

        //public async Task<string> UpdateProfile(Profile profile)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
