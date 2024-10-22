using Microsoft.ServiceFabric.Services.Remoting;

namespace Domain.Interfaces
{
    public interface ITransaction : IService
    {
        bool Prepare();
        void Commit();
        void Rollback();
    }
}
