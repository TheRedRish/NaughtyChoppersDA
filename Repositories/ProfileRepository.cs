using NaughtyChoppersDA.Entities;
using NaughtyChoppersDA.Globals;
using System.Data.SqlClient;
using System.Data;
using System.Data.SqlTypes;
using System.Reflection.PortableExecutable;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reflection.Metadata.Ecma335;

namespace NaughtyChoppersDA.Repositories
{
    public class ProfileRepository : IProfileRepository
    {
        private string myDbConnectionString = AccessToDb.ConnectionString;

        public void CreateProfile(Profile profile, User user)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(myDbConnectionString))
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

                    command.ExecuteNonQuery();
                }

                profile.ProfileId = GetProfileId(user.UserId);

                AddHobbyInterestsToProfile(profile.ProfileId, profile.HobbyInterests!);
                AddHelicopterModelInterestsToProfile(profile.ProfileId, profile.HelicopterModelInterests!);
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

        public void DeleteProfile(Guid? profileId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(myDbConnectionString))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand("DeleteProfile", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ProfileIdToDelete", SqlDbType.UniqueIdentifier).Value = profileId;
                    command.ExecuteNonQuery();
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

        public Profile GetProfileByProfileId(Guid profileId)
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
                            profile.Model = GetHelicopterModel(reader.GetInt32(3));
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
                profile.HelicopterModelInterests = GetHelicopterModelInterstsFromProfile(profile.ProfileId);
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

        public Profile? GetProfileByUserId(Guid? userId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(myDbConnectionString))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand("GetProfileByUserId", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserId", SqlDbType.UniqueIdentifier).Value = userId;
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Profile profile = new();
                            profile.ProfileId = reader.GetGuid(0);
                            profile.Name = reader.GetString(1);
                            profile.DateOfBirth = reader.GetDateTime(2);
                            profile.Model = GetHelicopterModel(reader.GetInt32(3));
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

                            profile.HobbyInterests = GetAllHobbyInterestsFromProfile(profile.ProfileId);
                            profile.HelicopterModelInterests = GetHelicopterModelInterstsFromProfile(profile.ProfileId);
                            if (profile.PostalCode != null)
                            {
                                profile.City = GetCityByPostalCode(profile.PostalCode);
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

        public Guid? GetProfileId(Guid? userId)
        {
            Guid? profileId = null;
            try
            {
                using (SqlConnection connection = new SqlConnection(myDbConnectionString))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand("GetProfileId", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserId", SqlDbType.UniqueIdentifier).Value = userId;
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            profileId = reader.GetGuid(0);
                        }
                    }
                }
                return profileId;
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

        public void UpdateProfile(Profile profile)
        {
            throw new NotImplementedException();
        }

        public string? GetCityByPostalCode(string postalCode)
        {
            string? city = null;
            try
            {
                using (SqlConnection connection = new SqlConnection(myDbConnectionString))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand("GetCityByPostalCode", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@PostalCode", SqlDbType.NVarChar).Value = postalCode;
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
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
        public List<HobbyInterest> GetAllHobbyInterestsFromProfile(Guid? profileId)
        {
            List<HobbyInterest> interestsList = new();
            try
            {
                using (SqlConnection connection = new SqlConnection(myDbConnectionString))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand("GetInterests", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ProfileId", SqlDbType.UniqueIdentifier).Value = profileId;
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
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

        public List<HobbyInterest> GetAllHobbyInterests()
        {
            List<HobbyInterest> interestsList = new();
            try
            {
                using (SqlConnection connection = new SqlConnection(myDbConnectionString))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand("GetAllInterests", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
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

        private void AddHobbyInterestsToProfile(Guid? profileId, List<HobbyInterest> hobbyInterests)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(myDbConnectionString))
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
                            command.ExecuteNonQuery();
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

        private void RemoveAllHobbyInterestsFromProfile(Guid? profileId)
        {
            throw new NotImplementedException();
        }

        private void RemoveHobbyInterestFromProfile(Guid? profileId, HobbyInterest hobbyInterest)
        {
            throw new NotImplementedException();
        }

        #endregion
        #region HelicopterModel
        public List<HelicopterModel> GetHelicopterModelInterstsFromProfile(Guid? profileId)
        {
            List<HelicopterModel> helicopterModelInterests = new();
            try
            {
                using (SqlConnection connection = new SqlConnection(myDbConnectionString))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand("GetModelInterest", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ProfileId", SqlDbType.UniqueIdentifier).Value = profileId;
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
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

        public HelicopterModel GetHelicopterModel(int? helicopterModelId)
        {
            HelicopterModel helicopterModel = new HelicopterModel();
            try
            {
                using (SqlConnection connection = new SqlConnection(myDbConnectionString))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand("GetHelicopterModel", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@HelicopterModelId", SqlDbType.Int).Value = helicopterModelId;
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
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

        public List<HelicopterModel> GetAllHelicoptersModels()
        {
            List<HelicopterModel> helicopterModelList = new();
            try
            {
                using (SqlConnection connection = new SqlConnection(myDbConnectionString))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand("GetAllModels", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
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

        private void AddHelicopterModelInterestsToProfile(Guid? profileId, List<HelicopterModel> helicopterModels)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(myDbConnectionString))
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
                            command.ExecuteNonQuery();
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

        private void RemoveAllHelicopterModelInterestsFromProfile(Guid? profileId)
        {
            throw new NotImplementedException();
        }

        private void RemoveHelicopterModelInterestFromProfile(Guid? profileId, HelicopterModel helicopterModel)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
