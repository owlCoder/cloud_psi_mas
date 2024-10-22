namespace Domain.Models
{
    public class User(string fullname, string email)
    {
        public string UserId { get; set; } = Guid.NewGuid().ToString().Replace("-", "");
        public required string Fullname { get; set; } = fullname;
        public required string Email { get; set; } = email;
    }
}
