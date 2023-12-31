﻿using NaughtyChoppersDA.Entities;
using NaughtyChoppersDA.Services;

namespace NaughtyChoppersDA.Repositories
{
    public interface IMatchingRepository
    {
        public Task<List<Profile>> GetFilteredListOfProfiles(Guid profileId);

        public Task LikeOrDislikeProfileAsync(Guid profileId, Guid likedProfileId, bool liked);

        public Task<List<Profile>> GetAllMatches(Guid profileId);

        public Task<List<Guid>> ListOfLikersAndLiked(Guid profileId);


    }
}