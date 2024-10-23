using Microsoft.ServiceFabric.Services.Remoting;

namespace Domain.Interfaces
{
    public interface IValidation : IService
    {
        Task<bool> Validate(string user_id, string book_id, uint quantity, double price_per_one);
    }
}
