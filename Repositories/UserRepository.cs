using NaughtyChoppersDA.Entities;
using NaughtyChoppersDA.Globals;
using NaughtyChoppersDA.Repositories;
using System;
using System.Data;
using System.Data.SqlClient;

namespace NaughtyChoppersDA.Repositories
{
    public class UserRepository : IUserRepository
    {
        private string myDbConnectionString = AccessToDb.ConnectionString;
        public async void CreateUser(string userName, string password)
        {
            try
            {
                await using (SqlConnection connection = new SqlConnection(myDbConnectionString))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand("AddUser", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Username", SqlDbType.NVarChar).Value = userName;
                    command.Parameters.AddWithValue("@Password", SqlDbType.NVarChar).Value = password;
                    command.ExecuteNonQuery();                 
                }
            }
            catch (SqlException ex)
            {
                // Depending on the specific exception, you can return different error reasons
                if (ex.Number == 2601 || ex.Number == 2627)
                {
                    throw new UserException("Duplicate username");
                }
                else
                {
                    throw new UserException("Database error");
                }
            }
            catch (Exception)
            {
                throw new UserException("Unknown error");
            }
        }

        public async void DeleteUser(User user)
        {
            try
            {
                await using (SqlConnection connection = new SqlConnection(myDbConnectionString))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand("RemoveUser", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Id", SqlDbType.UniqueIdentifier).Value = user.UserId;
                    command.ExecuteNonQuery();
                }
            }
            catch (SqlException)
            {
                {
                    throw new UserException("Database error");
                }
            }
            catch (Exception)
            {
                throw new UserException("Unknown error");
            }
        }

        //public async void UpdateUser(User user)
        //{
        //    throw new NotImplementedException();
        //}

        //public async Task<User> GetUserById(Guid? id)
        //{
        //    throw new NotImplementedException();
        //}

        public async Task<User?> GetUserByUsernameAndPassword(string userName, string password)
        {
            try
            {
                await using(SqlConnection connection = new SqlConnection(myDbConnectionString))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand("GetIdAndUserName", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@userName", SqlDbType.NVarChar).Value = userName;
                    command.Parameters.AddWithValue("@password", SqlDbType.NVarChar).Value = password;
                    using(SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            User user = new(reader.GetGuid(0), reader.GetString(1));
                            return user;
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

        public async Task<bool> DoesUserExist(string userName)
        {
            try
            {
                await using(SqlConnection connection = new SqlConnection(myDbConnectionString))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand("DoesUserExist", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@userName", SqlDbType.UniqueIdentifier).Value = userName;
                    using(SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return reader.GetInt32(0) == 1 ? true : false;
                        }
                    }
                }
                return false;
            }
            catch (SqlException ex)
            {
                string error = ex.Message;
                throw new UserException("Database error");                
            }
            catch (Exception)
            {
                throw new UserException("Unknown error");
            }
        }
    }
}
