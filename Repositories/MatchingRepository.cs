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

        private ProfileRepository profileRepo = new();

        public List<Profile> GetProfilesWithMatchingModelInterest(Guid profileId)
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
                                profiles.Add(GetProfile(reader.GetGuid(0)));
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

        public override Profile GetProfile(Guid? profileId)
        {
            Profile profile = new();
            try
            {
                using (SqlConnection connection = new SqlConnection(myDbConnectionString))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand("GetProfileByProfileId", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ProfileId", SqlDbType.UniqueIdentifier).Value = profileId;
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            profile.ProfileId = reader.GetGuid(0);
                            profile.Name = reader.GetString(1);
                            profile.DateOfBirth = reader.GetDateTime(2);
                            profile.Model = new();
                            profile.Model.Id = reader.GetInt32(3);
                            if (!reader.IsDBNull(4))
                            {
                                long bytesRead;
                                long fieldOffset = 0;
                                byte[] buffer = new byte[4096]; // Buffer to read chunks of data

                                using (var memoryStream = new System.IO.MemoryStream())
                                {
                                    do
                                    {
                                        bytesRead = reader.GetBytes(4, fieldOffset, buffer, 0, buffer.Length);
                                        memoryStream.Write(buffer, 0, (int)bytesRead);
                                        fieldOffset += bytesRead;
                                    }
                                    while (bytesRead > 0);

                                    byte[] varbinaryData = memoryStream.ToArray();

                                    profile.ProfileImage = varbinaryData;
                                }
                            }
                            profile.PostalCode = reader.GetString(5);
                        }
                    }
                }
                profile.HobbyInterests = GetAllHobbyInterestsFromProfile(profile.ProfileId);
                if (profile.PostalCode != null)
                {
                    profile.City = GetCityByPostalCode(profile.PostalCode);
                }
                return profile;
            }
            catch (SqlException)
            {
                throw new UserException("Database error");
            }
            catch (Exception)
            {
                throw new UserException("Unknown error");
            }
        }

        public List<Guid> ListOfLikersAndLiked(Guid profileId)
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
