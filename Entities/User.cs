namespace NaughtyChoppersDA.Entities
{
    public class User
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }

        public User(Guid userId, string userName)
        {
            UserId = userId;
            UserName = userName;
        }
    }
}
