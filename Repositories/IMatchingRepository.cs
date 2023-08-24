using NaughtyChoppersDA.Entities;
using NaughtyChoppersDA.Services;

namespace NaughtyChoppersDA.Repositories
{
    public interface IMatchingRepository
    {
        public List<Profile> GetProfilesWithMatchingModelInterest(Guid profileId);

        public List<Guid> ListOfLikersAndLiked(Guid profileId);

        public List<Profile> GetFilteredListOfProfiles(Guid profileId);
    }
}