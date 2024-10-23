using Domain.Models;
using Microsoft.ServiceFabric.Services.Remoting;

namespace Domain.Interfaces
{
    public interface IBookstore : IService, ITransaction
    {
        public Task<IEnumerable<Book>> ListAvailableItems();
        public Task EnlistPurchase(string book_id, uint count);
        public Task<double> GetItemPrice(string book_id);
    }
}
