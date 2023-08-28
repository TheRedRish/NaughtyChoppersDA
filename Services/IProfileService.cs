using NaughtyChoppersDA.Entities;

namespace NaughtyChoppersDA.Services
{
    public interface IProfileService
    {
        event EventHandler ProfileUpdated;

        Profile? Profile { get; set; }

        Task<Profile?> GetProfileByUserId(Guid userId);

        Task<Profile> GetProfileByProfileId(Guid profileId);

        Task<string> CreateProfile(Profile profile, User user);

        Task<string> DeleteProfile(Guid profileId);

        Task<List<HelicopterModel>> GetAllHelicopterModels();

        Task<List<HobbyInterest>> GetAllHobbyInterests();

        Task<string?> GetCityByPostalCode(string postalCode);

        //Task<string> UpdateProfile(Profile profile);
    }
}
