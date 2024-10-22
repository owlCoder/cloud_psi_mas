using Microsoft.ServiceFabric.Services.Remoting;

namespace Domain.Interfaces
{
    public interface ITransaction : IService
    {
        public bool Prepare();
        public void Commit();
        public void Rollback();
    }
}
