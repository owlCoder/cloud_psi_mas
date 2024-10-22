using Microsoft.ServiceFabric.Services.Remoting;

namespace Domain.Interfaces
{
    public interface IBank : IService, ITransaction
    {
        public void ListClients();
        public void EnlistMoneyTransfer(string user_id, double amount);
    }
}
