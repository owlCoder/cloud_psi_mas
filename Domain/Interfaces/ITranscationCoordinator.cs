using Microsoft.ServiceFabric.Services.Remoting;

namespace Domain.Interfaces
{
    public interface ITranscationCoordinator : IService
    {
        Task<bool> BuyBook(string user_id, string book_id, uint quantity, double price_per_one);
    }
}
