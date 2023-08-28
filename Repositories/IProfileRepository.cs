using NaughtyChoppersDA.Entities;

namespace NaughtyChoppersDA.Repositories
{
    public interface IProfileRepository
    {
        public Task CreateProfile(Profile profile, User user);

        public Task DeleteProfile(Guid? profileId); // TODO: Remember likes and chat

        public Task<List<HobbyInterest>> GetAllHobbyInterests();

        public Task<List<HobbyInterest>> GetAllHobbyInterestsFromProfile(Guid? profileId);

        public Task<List<HelicopterModel>> GetHelicopterModelInterstsFromProfile(Guid? profileId);

        public Task AddHelicopterModelInterestsToProfile(Guid? profileId, List<HelicopterModel> helicopterModels);

        public Task<List<HelicopterModel>> GetAllHelicoptersModels();

        public Task<HelicopterModel> GetHelicopterModel(int? helicopterModelId);

        public Task<string?> GetCityByPostalCode(string postalCode);

        public Task<Profile> GetProfileByProfileId(Guid profileId);

        public Task<Profile?> GetProfileByUserId(Guid? userId);

        public Task<Guid?> GetProfileId(Guid? userId);

        //void UpdateProfile(Profile profile);
    }
}
