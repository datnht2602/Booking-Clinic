using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Clinic.Data.Models;

namespace Clinic.Data.Store.Contracts
{
    public interface IServiceRepository : IBaseRepository<Service>
    {
        
    }
}