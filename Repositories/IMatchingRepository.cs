using NaughtyChoppersDA.Entities;
using NaughtyChoppersDA.Services;

namespace NaughtyChoppersDA.Repositories
{
    public interface IMatchingRepository
    {
        public List<Profile> GetFilteredListOfProfiles(Guid profileId);

        public void LikeProfileAsync(Guid profileId, Guid likedProfileId, bool? likedBack);

        public Task<List<Profile>> GetAllMatches(Guid? profileId);

	}
}