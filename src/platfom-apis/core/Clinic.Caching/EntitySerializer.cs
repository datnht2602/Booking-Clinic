using Clinic.Caching.Interfaces;
using Clinic.Common.Validator;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace Clinic.Caching
{
    [ExcludeFromCodeCoverage]
    public class EntitySerializer : IEntitySerializer
    {
        public async Task<T> DeserializeEntityAsync<T>(byte[] entity, CancellationToken cancellationToken = default)
        {
            ArgumentValidation.ThrowIfNull(entity);

            using MemoryStream memoryStream = new(entity);
            var value = await JsonSerializer.DeserializeAsync<T>(memoryStream, cancellationToken: cancellationToken);
            return value;
        }

        public async Task<byte[]> SerializeEntityAsync<T>(T entity, CancellationToken cancellationToken = default)
        {
            using MemoryStream memoryStream = new();
            await JsonSerializer.SerializeAsync(memoryStream,entity, cancellationToken: cancellationToken).ConfigureAwait(false);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream.ToArray();
        }
    }
}
