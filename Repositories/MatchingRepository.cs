using NaughtyChoppersDA.Entities;
using NaughtyChoppersDA.Globals;
using System.Data.SqlClient;
using System.Data;
using NaughtyChoppersDA.Services;

namespace NaughtyChoppersDA.Repositories
{
    public class MatchingRepository : ProfileRepository, IMatchingRepository
    {
        private string myDbConnectionString = AccessToDb.ConnectionString;

        private List<Profile> GetProfilesWithMatchingModelInterest(Guid profileId)
        {
            try
            {
                List<Profile> profiles = new List<Profile>();
                using (SqlConnection connection = new SqlConnection(myDbConnectionString))
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
                            while (reader.Read())
                            {
                                profiles.Add(GetProfileByProfileId(reader.GetGuid(0)));
                            }
                        }
                    }
                }
                return profiles;
            }
            catch (SqlException)
            {
                throw new Exception("Database Error");
            }
            catch (Exception)
            {
                throw new Exception("Unknown Error");
            }
        }

        private List<Guid> ListOfLikersAndLiked(Guid profileId)
        {
            try
            {
                List<Guid> profileIds = new();
                using (SqlConnection connection = new SqlConnection(myDbConnectionString))
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
                            while (reader.Read())
                            {
                                profileIds.Add(reader.GetGuid(0));
                            }
                        }

                        command.CommandText = "GetLikersId";
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                profileIds.Add(reader.GetGuid(0));
                            }
                        }
                    }
                }
                return profileIds;
            }
            catch (SqlException)
            {
                throw new Exception("Database Error");
            }
            catch (Exception)
            {
                throw new Exception("Unknown Error");
            }
        }

        public List<Profile> GetFilteredListOfProfiles(Guid profileId)
        {
            List<Profile> profileList = GetProfilesWithMatchingModelInterest(profileId);

            List<Guid> profileIds = ListOfLikersAndLiked(profileId);

            List<Profile> filteredList = new();

            foreach (Guid id in profileIds)
            {
                profileList.RemoveAll(x => x.ProfileId == id);
            }

            return filteredList;
        }
    }
}
