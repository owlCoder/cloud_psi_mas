using System.Fabric;
using Domain.Interfaces;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;

namespace Validation
{
    internal sealed class Validation(StatelessServiceContext context) : StatelessService(context), IValidation
    {
        ITranscationCoordinator coordinator = ServiceProxy.Create<ITranscationCoordinator>(new Uri("fabric:/CloudSF/TransactionController"));

        public async Task<bool> Validate(string user_id, string book_id, uint quantity, double price_per_one)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(user_id) || quantity < 1 || string.IsNullOrWhiteSpace(book_id) || price_per_one < 0)
                    return false;

                return await coordinator.BuyBook(user_id, book_id, quantity, price_per_one);
            }
            catch
            {
                return await Task.FromResult(false);
            }
        }

        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners() => this.CreateServiceRemotingInstanceListeners();
    }
}
