namespace NotificationApp.Entities
{
    public class User
    {
        public int Id { get; set; }
        public required string Email { get; set; }
        public bool IsActive { get; set; }
    }
}