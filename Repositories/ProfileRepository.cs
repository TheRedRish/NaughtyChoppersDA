using NaughtyChoppersDA.Entities;
using NaughtyChoppersDA.Globals;
using System.Data.SqlClient;
using System.Data;
using System.Data.SqlTypes;
using System.Reflection.PortableExecutable;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;

namespace NaughtyChoppersDA.Repositories
{
    //Rasmus Note: Spørg Niels om ExecuteNonQueryAsync, Reader.ReadAsync og ExecuteReaderAsync er practical eller om det er en waste!
    public class ProfileRepository : IProfileRepository
    {
        private string myDbConnectionString = AccessToDb.ConnectionString;

        public async Task CreateProfile(Profile profile, User user)
        {
            try
            {
                await using (SqlConnection connection = new SqlConnection(myDbConnectionString))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand("CreateProfile", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Name", SqlDbType.NVarChar).Value = profile.Name;
                    command.Parameters.AddWithValue("@BirthDate", SqlDbType.Date).Value = profile.DateOfBirth;
                    command.Parameters.AddWithValue("@ModelId", SqlDbType.Int).Value = profile.Model?.Id;
                    command.Parameters.AddWithValue("@ProfileImg", SqlDbType.VarBinary).Value = profile.ProfileImage;
                    command.Parameters.AddWithValue("@PostalCode", SqlDbType.NVarChar).Value = profile.PostalCode;
                    command.Parameters.AddWithValue("@UserId", SqlDbType.UniqueIdentifier).Value = user.UserId;

                    await command.ExecuteNonQueryAsync();
                }

                profile.ProfileId = await GetProfileId(user.UserId);

                await AddHobbyInterestsToProfile(profile.ProfileId, profile.HobbyInterests!);
                await AddHelicopterModelInterestsToProfile(profile.ProfileId, profile.HelicopterModelInterests!);
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

        public async Task DeleteProfile(Guid profileId)
        {
            try
            {
                await using (SqlConnection connection = new SqlConnection(myDbConnectionString))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand("DeleteProfile", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ProfileIdToDelete", SqlDbType.UniqueIdentifier).Value = profileId;
                    await command.ExecuteNonQueryAsync();
                }
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

        public async Task<Profile> GetProfileByProfileId(Guid profileId)
        {
            Profile profile = new();
            try
            {
                await using (SqlConnection connection = new SqlConnection(myDbConnectionString))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand("GetProfileByProfileId", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ProfileId", SqlDbType.UniqueIdentifier).Value = profileId;
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (await reader.ReadAsync())
                        {
                            profile.ProfileId = reader.GetGuid(0);
                            profile.Name = reader.GetString(1);
                            profile.DateOfBirth = reader.GetDateTime(2);
                            profile.Model = await GetHelicopterModel(reader.GetInt32(3));
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
                if(profile.ProfileId != null)
                {
                    profile.HobbyInterests = await GetAllHobbyInterestsFromProfile(profile.ProfileId);
                    profile.HelicopterModelInterests = await GetHelicopterModelInterstsFromProfile(profile.ProfileId);
                    if (profile.PostalCode != null)
                    {
                        profile.City = await GetCityByPostalCode(profile.PostalCode);
                    }
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

        public async Task<Profile?> GetProfileByUserId(Guid userId)
        {
            try
            {
                await using (SqlConnection connection = new SqlConnection(myDbConnectionString))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand("GetProfileByUserId", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserId", SqlDbType.UniqueIdentifier).Value = userId;
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (await reader.ReadAsync())
                        {
                            Profile profile = new();
                            profile.ProfileId = reader.GetGuid(0);
                            profile.Name = reader.GetString(1);
                            profile.DateOfBirth = reader.GetDateTime(2);
                            profile.Model = await GetHelicopterModel(reader.GetInt32(3));
                            if (!await reader.IsDBNullAsync(4))
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

                            profile.HobbyInterests = await GetAllHobbyInterestsFromProfile(profile.ProfileId);
                            profile.HelicopterModelInterests = await GetHelicopterModelInterstsFromProfile(profile.ProfileId);
                            if (profile.PostalCode != null)
                            {
                                profile.City = await GetCityByPostalCode(profile.PostalCode);
                            }
                            return profile;
                        }
                    }
                }
                return null;
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

        public async Task<Guid> GetProfileId(Guid userId)
        {
            try
            {
                await using (SqlConnection connection = new SqlConnection(myDbConnectionString))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand("GetProfileId", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserId", SqlDbType.UniqueIdentifier).Value = userId;
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (await reader.ReadAsync())
                        {
                            return reader.GetGuid(0);
                        }
                    }
                }
                return Guid.Empty;
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

        public async Task<string?> GetCityByPostalCode(string postalCode)
        {
            string? city = null;
            try
            {
                await using (SqlConnection connection = new SqlConnection(myDbConnectionString))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand("GetCityByPostalCode", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@PostalCode", SqlDbType.NVarChar).Value = postalCode;
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (await reader.ReadAsync())
                        {
                            city = reader.GetString(0);
                        }
                    }
                }
            }
            catch (SqlException)
            {
                throw new UserException("Database error");
            }
            catch (Exception)
            {
                throw new UserException("Unknown error");
            }

            return city;
        }

        #region HobbyInterests
        public async Task<List<HobbyInterest>> GetAllHobbyInterestsFromProfile(Guid profileId)
        {
            List<HobbyInterest> interestsList = new();
            try
            {
                await using (SqlConnection connection = new SqlConnection(myDbConnectionString))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand("GetInterests", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ProfileId", SqlDbType.UniqueIdentifier).Value = profileId;
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (await reader.ReadAsync())
                        {
                            interestsList.Add(new HobbyInterest { Id = reader.GetInt32(0), Name = reader.GetString(1) });
                        }
                    }
                }
            }
            catch (SqlException)
            {
                throw new UserException("Database error");
            }
            catch (Exception)
            {
                throw new UserException("Unknown error");
            }
            return interestsList;
        }

        public async Task<List<HobbyInterest>> GetAllHobbyInterests()
        {
            List<HobbyInterest> interestsList = new();
            try
            {
                await using (SqlConnection connection = new SqlConnection(myDbConnectionString))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand("GetAllInterests", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (await reader.ReadAsync())
                        {
                            interestsList.Add(new HobbyInterest { Id = reader.GetInt32(0), Name = reader.GetString(1) });
                        }
                    }
                }
            }
            catch (SqlException)
            {
                throw new UserException("Database error");
            }
            catch (Exception)
            {
                throw new UserException("Unknown error");
            }
            return interestsList; ;
        }

        private async Task AddHobbyInterestsToProfile(Guid profileId, List<HobbyInterest> hobbyInterests)
        {
            try
            {
                await using (SqlConnection connection = new SqlConnection(myDbConnectionString))
                {
                    connection.Open();
                    foreach (HobbyInterest hobbyInterest in hobbyInterests)
                    {
                        try
                        {
                            SqlCommand command = new SqlCommand("AddInterestToProfile", connection);
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@ProfileId", SqlDbType.UniqueIdentifier).Value = profileId;
                            command.Parameters.AddWithValue("@InterestId", SqlDbType.Int).Value = hobbyInterest.Id;
                            await command.ExecuteNonQueryAsync();
                        }
                        catch (SqlException ex)
                        {
                            // Ignore errors related to unique Id
                            if (ex.Number != 2601 && ex.Number != 2627)
                            {
                                throw ex;
                            }
                        }
                    }
                }
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

        //private void RemoveAllHobbyInterestsFromProfile(Guid? profileId)
        //{
        //    throw new NotImplementedException();
        //}

        //private void RemoveHobbyInterestFromProfile(Guid? profileId, HobbyInterest hobbyInterest)
        //{
        //    throw new NotImplementedException();
        //}

        #endregion
        #region HelicopterModel
        public async Task<List<HelicopterModel>> GetHelicopterModelInterstsFromProfile(Guid profileId)
        {
            List<HelicopterModel> helicopterModelInterests = new();
            try
            {
                await using (SqlConnection connection = new SqlConnection(myDbConnectionString))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand("GetModelInterest", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ProfileId", SqlDbType.UniqueIdentifier).Value = profileId;
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (await reader.ReadAsync())
                        {
                            helicopterModelInterests.Add(new HelicopterModel { Id = reader.GetInt32(0), Name = reader.GetString(1) });
                        }
                    }
                }
            }
            catch (SqlException)
            {
                throw new UserException("Database error");
            }
            catch (Exception)
            {
                throw new UserException("Unknown error");
            }
            return helicopterModelInterests;
        }

        public async Task<HelicopterModel> GetHelicopterModel(int? helicopterModelId)
        {
            HelicopterModel helicopterModel = new HelicopterModel();
            try
            {
                await using (SqlConnection connection = new SqlConnection(myDbConnectionString))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand("GetHelicopterModel", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@HelicopterModelId", SqlDbType.Int).Value = helicopterModelId;
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (await reader.ReadAsync())
                        {
                            helicopterModel.Id = helicopterModelId;
                            helicopterModel.Name = reader.GetString(0);
                        }
                    }
                }
                return helicopterModel;
            }
            catch (SqlException)
            {
                throw new UserException("Database error");
            }
            catch (Exception)
            {
                throw new UserException("Unknown error");
            }

        }// TODO: might be redundant

        public async Task<List<HelicopterModel>> GetAllHelicoptersModels()
        {
            List<HelicopterModel> helicopterModelList = new();
            try
            {
                await using (SqlConnection connection = new SqlConnection(myDbConnectionString))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand("GetAllModels", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (await reader.ReadAsync())
                        {
                            helicopterModelList.Add(new HelicopterModel { Id = reader.GetInt32(0), Name = reader.GetString(1) });
                        }
                    }
                }
            }
            catch (SqlException)
            {
                throw new UserException("Database error");
            }
            catch (Exception)
            {
                throw new UserException("Unknown error");
            }
            return helicopterModelList;
        }

        public async Task AddHelicopterModelInterestsToProfile(Guid profileId, List<HelicopterModel> helicopterModels)
        {
            try
            {
                await using (SqlConnection connection = new SqlConnection(myDbConnectionString))
                {
                    connection.Open();
                    foreach (HelicopterModel helicopterModel in helicopterModels)
                    {
                        try
                        {
                            SqlCommand command = new SqlCommand("AddModelInterestToProfile", connection);
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@ProfileId", SqlDbType.UniqueIdentifier).Value = profileId;
                            command.Parameters.AddWithValue("@ModelId", SqlDbType.Int).Value = helicopterModel.Id;
                            await command.ExecuteNonQueryAsync();
                        }
                        catch (SqlException ex)
                        {
                            // Ignore errors related to unique Id
                            if (ex.Number != 2601 && ex.Number != 2627)
                            {
                                throw ex;
                            }
                        }
                    }
                }
            }
            catch (SqlException se)
            {
                throw new UserException("Database error");
            }
            catch (Exception)
            {
                throw new UserException("Unknown error");
            }
        }

        #endregion

        //public void UpdateProfile(Profile profile)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
