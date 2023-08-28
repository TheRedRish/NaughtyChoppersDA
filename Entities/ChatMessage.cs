namespace NaughtyChoppersDA.Entities
{
    public class ChatMessage
    {
        public string Message { get; set; }
        public Profile Sender { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
