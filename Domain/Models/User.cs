namespace Domain.Models
{
    public class User(string fullname, string email)
    {
        public string UserId { get; set; } = Guid.NewGuid().ToString().Replace("-", "");
        public string Fullname { get; set; } = fullname;
        public string Email { get; set; } = email;
        public double Balance { get; set; } = new Random().NextDouble() * 1000;
    }
}
