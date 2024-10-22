using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Book
    {
        public required string BookId { get; set; }
        public required string Title { get; set; }
        public required string Author { get; set; }
        public uint PagesInTotal { get; set; }
        public double Price { get; set; }
        public uint Quantity { get; set; }

        public Book()
        {
            BookId = Guid.NewGuid().ToString().Replace("-", "");
            PagesInTotal = ((uint)new Random().Next(50, 100));
            Price = new Random().NextDouble() * 100;
            Quantity = ((uint)new Random().Next(1, 10));
        }
    }
}
