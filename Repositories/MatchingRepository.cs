using NaughtyChoppersDA.Entities;
using NaughtyChoppersDA.Globals;
using System.Data.SqlClient;
using System.Data;
using NaughtyChoppersDA.Services;
using System;
using System.Collections.Generic;

namespace NaughtyChoppersDA.Repositories
{
    public class MatchingRepository : ProfileRepository, IMatchingRepository
    {
        private string myDbConnectionString = AccessToDb.ConnectionString;

        public async Task<List<Profile>> GetProfilesWithMatchingModelInterest(Guid profileId)
        {
            try
            {
                List<Profile> profiles = new List<Profile>();
                await using (SqlConnection connection = new SqlConnection(myDbConnectionString))
                {
                    connection.Open();

                    string procedureName = "GetProfileModelInterest";

                    using (SqlCommand command = new SqlCommand(procedureName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Add parameters to the stored procedure
                        command.Parameters.Add("@ProfileId", SqlDbType.UniqueIdentifier).Value = profileId;

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (await reader.ReadAsync())
                            {
                                profiles.Add(await GetProfileByProfileId(reader.GetGuid(0)));
                            }
                        }
                    }
                }
                return profiles;
            }
            catch (SqlException se)
            {
                throw new Exception($"Database Error: {se.Message}");
            }
            catch (Exception e)
            {
                throw new Exception($"Unknown Error: {e.Message}");
            }
        }

        public async Task<List<Guid>> ListOfLikersAndLiked(Guid profileId)
        {
            try
            {
                List<Guid> profileIds = new();
                await using (SqlConnection connection = new SqlConnection(myDbConnectionString))
                {
                    connection.Open();

                    string procedureName = "GetLikedId";

                    using (SqlCommand command = new SqlCommand(procedureName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Add parameters to the stored procedure
                        command.Parameters.Add("@ProfileId", SqlDbType.UniqueIdentifier).Value = profileId;

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (await reader.ReadAsync())
                            {
                                profileIds.Add(reader.GetGuid(0));
                            }
                        }

                        command.CommandText = "GetLikersId";
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (await reader.ReadAsync())
                            {
                                profileIds.Add(reader.GetGuid(0));
                            }
                        }
                    }
                }
                return profileIds;
            }
            catch (SqlException se)
            {
                throw new Exception($"Database Error: {se.Message}");
            }
            catch (Exception e)
            {
                throw new Exception($"Unknown Error: {e.Message}");
            }
        }

        public async Task<List<Profile>> GetFilteredListOfProfiles(Guid profileId)
        {
            List<Profile> profileList = await GetProfilesWithMatchingModelInterest(profileId);

            List<Guid> profileIds = await ListOfLikersAndLiked(profileId);

            foreach (Guid id in profileIds)
            {
                profileList.RemoveAll(x => x.ProfileId == id);
            }
            ScrambleList(profileList);
            return profileList;
        }

        private static void ScrambleList<T>(List<T> list)
        {
            Random random = new Random();
            int n = list.Count;
            for (int i = n - 1; i > 0; i--)
            {
                int j = random.Next(0, i + 1);
                T temp = list[i];
                list[i] = list[j];
                list[j] = temp;
            }
        }

        public async Task LikeOrDislikeProfileAsync(Guid myProfileId, Guid theirProfileId, bool liked)
        {
            try
            {
                await using(SqlConnection connection = new SqlConnection(myDbConnectionString))
                {
                    connection.Open();

                    string procedureName = "SetLikeTableResult";

                    using (SqlCommand command = new SqlCommand(procedureName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Add parameters to the stored procedure
                        command.Parameters.Add("@SenderId", SqlDbType.UniqueIdentifier).Value = myProfileId;
                        command.Parameters.Add("@ReceiverId", SqlDbType.UniqueIdentifier).Value = theirProfileId;
                        command.Parameters.Add("@Liked", SqlDbType.Bit).Value = liked;
                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (SqlException se)
            {
                throw new Exception($"Database Error: {se.Message}");
            }
            catch (Exception e)
            {
                throw new Exception($"Unknown Error: {e.Message}");
            }
        }

        public async Task<List<Profile>> GetAllMatches(Guid profileId)
        {
			try
			{
				List<Profile> profiles = new();
				using (SqlConnection connection = new SqlConnection(myDbConnectionString))
				{
					connection.Open();

					string procedureName = "GetLikedMatches";

					using (SqlCommand command = new SqlCommand(procedureName, connection))
					{
						command.CommandType = CommandType.StoredProcedure;

						// Add parameters to the stored procedure
						command.Parameters.Add("@YourId", SqlDbType.UniqueIdentifier).Value = profileId;

						await using (SqlDataReader reader = command.ExecuteReader())
						{
							while (await reader.ReadAsync())
							{
								profiles.Add(await GetProfileByProfileId(reader.GetGuid(0)));
							}
						}
					}
				}
				return profiles;
			}
			catch (SqlException se)
			{
				throw new Exception($"Database Error: {se.Message}");
			}
			catch (Exception e)
			{
				throw new Exception($"Unknown Error: {e.Message}");
			}
		}
    }
}
