namespace Domain.Models
{
    public class Book
    {
        public string BookId { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public uint PagesInTotal { get; set; }
        public double Price { get; set; }
        public uint Quantity { get; set; }

        public Book()
        {
            BookId = Guid.NewGuid().ToString().Replace("-", "");
            Title = "/";
            Author = "/";
            PagesInTotal = (uint)new Random().Next(50, 100);
            Price = new Random().NextDouble() * 100;
            Quantity = (uint)new Random().Next(1, 10);
        }

        public Book(string title, string author)
        {
            BookId = Guid.NewGuid().ToString().Replace("-", "");
            Title = title;
            Author = author;
            PagesInTotal = (uint)new Random().Next(50, 100);
            Price = new Random().NextDouble() * 100;
            Quantity = (uint)new Random().Next(1, 10);
        }
    }
}
