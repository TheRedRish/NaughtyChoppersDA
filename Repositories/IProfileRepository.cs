using NaughtyChoppersDA.Entities;

namespace NaughtyChoppersDA.Repositories
{
    public interface IProfileRepository
    {
        void CreateProfile(Profile profile, User user);
        void DeleteProfile(Guid? profileId); // TODO: Remember likes and chat
        List<HobbyInterest> GetAllHobbyInterests();
        List<HobbyInterest> GetAllHobbyInterestsFromProfile(Guid? profileId);
        List<HelicopterModel> GetHelicopterModelInterstsFromProfile(Guid? profileId);
        List<HelicopterModel> GetAllHelicoptersModels();
        HelicopterModel GetHelicopterModel(int helicopterModelId);
        string? GetCityByPostalCode(string postalCode);

        Profile GetProfileByProfileId(Guid profileId);

        Profile GetProfileByUserId(Guid? userId);
        Guid? GetProfileId(Guid? userId);
        void UpdateProfile(Profile profile);
    }
}
