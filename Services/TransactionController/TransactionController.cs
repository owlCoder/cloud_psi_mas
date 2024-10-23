using System.Fabric;
using Domain.Interfaces;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Communication.Client;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;

namespace TransactionController
{
    internal sealed class TransactionController(StatelessServiceContext context) : StatelessService(context), ITranscationCoordinator
    {
        IBank bank = ServiceProxy.Create<IBank>(new Uri("fabric:/CloudSF/Bank"), new ServicePartitionKey(0), TargetReplicaSelector.Default);
        IBookstore store = ServiceProxy.Create<IBookstore>(new Uri("fabric:/CloudSF/Bookstore"), new ServicePartitionKey(0), TargetReplicaSelector.Default);

        public async Task<bool> BuyBook(string user_id, string book_id, uint quantity, double price_per_one)
        {
            try
            {
                await store.EnlistPurchase(book_id, quantity);
                await bank.EnlistMoneyTransfer(user_id, quantity * price_per_one);

                if (await store.Prepare() && await bank.Prepare())
                {
                    if (await store.Commit())
                    {
                        if (await bank.Commit())
                            return true;

                        await store.Rollback();
                        await bank.Rollback();
                        return false;
                    }
                }

                await store.Rollback();
                await bank.Rollback();

                return false;
            }
            catch
            {
                return false;
            }
        }

        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners() => this.CreateServiceRemotingInstanceListeners();
    }
}
