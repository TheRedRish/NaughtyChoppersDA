using NaughtyChoppersDA.Entities;
using NaughtyChoppersDA.Services;

namespace NaughtyChoppersDA.Repositories
{
    public interface IMatchingRepository
    {
        public List<Profile> GetFilteredListOfProfiles(Guid profileId);
    }
}