using Microsoft.ServiceFabric.Services.Remoting;

namespace Domain.Interfaces
{
    public interface ITransaction : IService
    {
        Task<bool> Prepare();
        Task<bool> Commit();
        Task Rollback();
    }
}
