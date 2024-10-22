namespace Domain.Models
{
    public class BankAccount(string user_id)
    {
        public string AccountId { get; set; } = Guid.NewGuid().ToString().Replace("-", "");
        public double Balance { get; set; } = new Random().NextDouble() * 1000;
        public string UserId { get; set; } = user_id;
    }
}
