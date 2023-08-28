using NaughtyChoppersDA.Pages;
using NaughtyChoppersDA.Entities;

namespace NaughtyChoppersDA.Repositories
{
    public interface IChatRepository
    {
        public Task<List<ChatMessage>> GetAllChatMessages(Guid senderId, Guid ReceiverId);

        public Task SendMessage(Guid senderId, Guid ReceiverId, string message);

        public Task<List<ChatMessage>> UpdateChatAsync(Guid senderId, Guid ReceiverId, List<ChatMessage> currentChat);

    }
}
