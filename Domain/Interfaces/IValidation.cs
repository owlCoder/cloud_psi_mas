using Microsoft.ServiceFabric.Services.Remoting;

namespace Domain.Interfaces
{
    public interface IValidation : IService
    {
        Task<bool> Validate(string user_id, int quantity);
    }
}
