using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Caching.Interfaces
{
    public interface IEntitySerializer
    {
        Task<byte[]> SerializeEntityAsync<T>(T entity,CancellationToken cancellationToken = default);
        Task<T> DeserializeEntityAsync<T>(byte[] entity, CancellationToken cancellationToken = default);
    }
}
