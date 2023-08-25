using NaughtyChoppersDA.Entities;

namespace NaughtyChoppersDA.Services
{
    public interface IProfileService
    {
        Profile? Profile { get; set; }
        Profile GetProfileByUserId(Guid userId);
        Profile GetProfileByProfileId(Guid profileId);
        string CreateProfile(Profile profile, User user);
        string UpdateProfile(Profile profile);
        string DeleteProfile(Guid profileId);
        List<HelicopterModel> GetAllHelicopterModels();
        List<HobbyInterest>GetAllHobbyInterests();
        string? GetCityByPostalCode(string postalCode);
    }
}
