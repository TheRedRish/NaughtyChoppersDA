using NaughtyChoppersDA.Entities;
using NaughtyChoppersDA.Globals;
using System.Data.SqlClient;
using System.Data;
using NaughtyChoppersDA.Repositories;

namespace NaughtyChoppersDA.Repositories
{
    public class ChatRepository : ProfileRepository, IChatRepository
    {
        private string myDbConnectionString = AccessToDb.ConnectionString;

        public async Task<List<ChatMessage>> GetAllChatMessages(Guid senderId, Guid receiverId)
        {
            try
            {
                List<ChatMessage> chatMessages = new();
                using (SqlConnection connection = new SqlConnection(myDbConnectionString))
                {
                    connection.Open();

                    string procedureName = "GetAllMessagesFromChat";

                    using (SqlCommand command = new SqlCommand(procedureName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Add parameters to the stored procedure
                        command.Parameters.Add("@Sender", SqlDbType.UniqueIdentifier).Value = senderId;
                        command.Parameters.Add("@Receiver", SqlDbType.UniqueIdentifier).Value = receiverId;

                        await using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ChatMessage chatMessage = new();
                                Guid guid = reader.GetGuid(0);
                                chatMessage.Sender = GetProfileByProfileId(reader.GetGuid(0));
                                chatMessage.Message = reader.GetString(1);
                                chatMessage.Timestamp = reader.GetDateTime(2);
                                chatMessages.Add(chatMessage);
                            }
                        }
                    }
                }
                return chatMessages;
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

        public async void SendMessage(Guid senderId, Guid receiverId, string message)
        {
            try
            {
                List<Profile> profiles = new List<Profile>();
                using (SqlConnection connection = new SqlConnection(myDbConnectionString))
                {
                    connection.Open();

                    string procedureName = "SendMessageToReceiver";

                    using (SqlCommand command = new SqlCommand(procedureName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Add parameters to the stored procedure
                        command.Parameters.Add("@Sender", SqlDbType.UniqueIdentifier).Value = senderId;
                        command.Parameters.Add("@Receiver", SqlDbType.UniqueIdentifier).Value = receiverId;
                        command.Parameters.Add("@ChatMessage", SqlDbType.NVarChar).Value = message;

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

        public async Task<List<ChatMessage>> UpdateChatAsync(Guid senderId, Guid receiverId, List<ChatMessage> currentChat)
        {
            try
            {
                ChatMessage chatMessage = new();
                using (SqlConnection connection = new SqlConnection(myDbConnectionString))
                {
                    connection.Open();

                    string procedureName = "GetAllMessagesFromChat";

                    using (SqlCommand command = new SqlCommand(procedureName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Add parameters to the stored procedure
                        command.Parameters.Add("@Sender", SqlDbType.UniqueIdentifier).Value = senderId;
                        command.Parameters.Add("@Receiver", SqlDbType.UniqueIdentifier).Value = receiverId;
                        command.Parameters.Add("@AmountOfSkips", SqlDbType.Int).Value = currentChat.Count;

                        await using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                chatMessage.Sender = GetProfileByProfileId(reader.GetGuid(0));
                                chatMessage.Message = reader.GetString(1);
                                chatMessage.Timestamp = reader.GetDateTime(2);
                                currentChat.Add(chatMessage);
                            }
                        }
                    }
                }
                return currentChat;
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
