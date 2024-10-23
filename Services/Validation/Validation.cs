using System.Fabric;
using Domain.Interfaces;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;

namespace Validation
{
    internal sealed class Validation(StatelessServiceContext context) : StatelessService(context), IValidation
    {
        public async Task<bool> Validate(string user_id, int quantity)
        {
            try
            {
                if(string.IsNullOrWhiteSpace(user_id) || quantity < 1)
                    return false;

                return true;
            }
            catch
            {
                return await Task.FromResult(false);
            }
        }

        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners() => this.CreateServiceRemotingInstanceListeners();
    }
}
