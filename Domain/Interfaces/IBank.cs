using Domain.Models;
using Microsoft.ServiceFabric.Services.Remoting;

namespace Domain.Interfaces
{
    public interface IBank : IService, ITransaction
    {
        public Task<IEnumerable<User>> ListClients();
        public Task EnlistMoneyTransfer(string user_id, double amount);
    }
}
