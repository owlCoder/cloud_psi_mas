using Microsoft.ServiceFabric.Services.Remoting;

namespace Domain.Interfaces
{
    public interface IBookstore : IService, ITransaction
    {
        public void ListAvailableItems();
        public void EnlistPurchase(string book_id, uint count);
        public double GetItemPrice(string book_id);
    }
}
