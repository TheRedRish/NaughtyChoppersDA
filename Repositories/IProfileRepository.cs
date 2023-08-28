using NaughtyChoppersDA.Entities;

namespace NaughtyChoppersDA.Repositories
{
    public interface IProfileRepository
    {
        Task CreateProfile(Profile profile, User user);

        Task DeleteProfile(Guid? profileId); // TODO: Remember likes and chat

        Task<List<HobbyInterest>> GetAllHobbyInterests();

        Task<List<HobbyInterest>> GetAllHobbyInterestsFromProfile(Guid? profileId);

        Task<List<HelicopterModel>> GetHelicopterModelInterstsFromProfile(Guid? profileId);

        Task AddHelicopterModelInterestsToProfile(Guid? profileId, List<HelicopterModel> helicopterModels);

        Task<List<HelicopterModel>> GetAllHelicoptersModels();

        Task<HelicopterModel> GetHelicopterModel(int? helicopterModelId);

        Task<string?> GetCityByPostalCode(string postalCode);

        Task<Profile> GetProfileByProfileId(Guid profileId);

        Task<Profile?> GetProfileByUserId(Guid? userId);

        Task<Guid?> GetProfileId(Guid? userId);

        //void UpdateProfile(Profile profile);
    }
}
