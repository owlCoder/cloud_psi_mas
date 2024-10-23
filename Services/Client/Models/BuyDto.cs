namespace Client.Models
{
    public class BuyDto
    {
        public string UserId { get; set; } = string.Empty;

        public string BookId { get; set; } = string.Empty;

        public int Quantity { get; set; }

        public double PricePerPC { get; set; }

        public BuyDto() { }
    }
}
