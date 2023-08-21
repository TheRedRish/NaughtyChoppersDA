using NaughtyChoppersDA.Entities;

namespace NaughtyChoppersDA.Services
{
    public interface IProfileService
    {
        Profile? Profile { get; set; }
        User GetProfile(Guid id);
        string CreateProfile(Profile profile, User user);
        string UpdateProfile(Profile profile);
        string DeleteProfile(Guid id);
        List<HelicopterModel> GetAllHelicopterModels();
        string? GetCityByPostalCode(string postalCode);
    }
}
