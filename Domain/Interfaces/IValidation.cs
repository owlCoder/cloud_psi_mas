using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Remoting;

namespace Domain.Interfaces
{
    public interface IValidation : IService
    {
        Task<bool> Validate(string user_id, int quantity);
    }
}
