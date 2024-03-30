using System.Net.Mime;
using System.Text;
using System.Text.Json;
using AutoMapper;
using Clinic.Caching.Interfaces;
using Clinic.Common.Models;
using Clinic.Common.Options;
using Clinic.Common.Validator;
using Clinic.Data.Models;
using Clinic.DTO.Models;
using Clinic.DTO.Models.Dto;
using Clinic.Product.Contracts;
using Microsoft.Extensions.Options;

namespace Clinic.Product.Services
{
    public class ProductService : IProductService
    {
        private const string ContentType = "application/json";
        private readonly IOptions<ApplicationSettings> applicationSettings;
        private readonly HttpClient httpClient;
        private readonly IMapper autoMapper;
        private readonly IDistributedCacheService cacheService;
        public ProductService(IHttpClientFactory httpClientFactory, IOptions<ApplicationSettings> applicationSettings,
        IMapper autoMapper, IDistributedCacheService cacheService)
        {
            ArgumentNullException.ThrowIfNull(applicationSettings,nameof(applicationSettings));
            this.applicationSettings = applicationSettings;
            this.httpClient = httpClientFactory.CreateClient();
            this.autoMapper = autoMapper;
            this.cacheService = cacheService;
        }
        public async Task<ResponseDto> AddProductAsync(Data.Models.Product product)
        {
            ArgumentValidation.ThrowIfNull(product);
            ResponseDto result = new();
            var existingProduct = await GetProductByIdASync(productId: product.Id);
            if (existingProduct != null && existingProduct.IsSuccess)
            {
                 return await UpdateProductAsync(product);
            }
            product.Id = Guid.NewGuid().ToString();
            product.CreatedDate = DateTime.UtcNow;
            using var productRequest = new StringContent(JsonSerializer.Serialize(product),Encoding.UTF8,ContentType);
            var productResponse = await httpClient.PostAsync(new Uri($"{this.applicationSettings.Value.DataStoreEndpoint}getproduct"),productRequest).ConfigureAwait(false);

            if(!productResponse.IsSuccessStatusCode){
                await ThrowServiceToServiceErrors(productResponse).ConfigureAwait(false);
            }
            var createdProductDAO = await productResponse.Content.ReadFromJsonAsync<Data.Models.Product>().ConfigureAwait(false);

            await cacheService.RemoveCacheAsync("products").ConfigureAwait(false);
            if(createdProductDAO != null)
            {
                result.Result = createdProductDAO;
            }
            return result;
        }

        public async Task<ResponseDto> DeleteProductAsync(string productId)
        {
            var productResponse = await httpClient.DeleteAsync(new Uri($"{this.applicationSettings.Value.DataStoreEndpoint}getproduct/{productId}")).ConfigureAwait(false);
            if(!productResponse.IsSuccessStatusCode){
                await this.ThrowServiceToServiceErrors(productResponse).ConfigureAwait(false);
            }
            await cacheService.RemoveCacheAsync("products").ConfigureAwait(false);
            return new ResponseDto();
        }

        public async Task<ResponseDto> GetProductByIdASync(string productId)
        {
            using var productRequest = new HttpRequestMessage(HttpMethod.Get,$"{this.applicationSettings.Value.DataStoreEndpoint}getproduct/{productId}");
            var productResponse = await httpClient.SendAsync(productRequest).ConfigureAwait(false);
            ResponseDto result = new();
            if(!productResponse.IsSuccessStatusCode){
                await ThrowServiceToServiceErrors(productResponse).ConfigureAwait(false);
            }
            if(productResponse.StatusCode != System.Net.HttpStatusCode.NoContent){
                var productDAO = await productResponse.Content.ReadFromJsonAsync<Clinic.Data.Models.Product>().ConfigureAwait(false);
                result.Result = productDAO;
                return result;
            }else{
                return result;
            }
        }

        public async Task<ResponseDto> GetProductsAsync(string filterCriteria = null)
        {
            var products = await this.cacheService.GetCacheAsync<IEnumerable<Clinic.Data.Models.Product>>($"products{filterCriteria}").ConfigureAwait(false);
            if(products == null)
            {
                using var productRequest = new HttpRequestMessage(HttpMethod.Get, $"{this.applicationSettings.Value.DataStoreEndpoint}getallproducts?filterCriteria={filterCriteria}");
                var productResponse = await this.httpClient.SendAsync(productRequest).ConfigureAwait(false);
                if(!productResponse.IsSuccessStatusCode)
                {
                    await this.ThrowServiceToServiceErrors(productResponse).ConfigureAwait(false);
                }
                if(productResponse.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    return new ResponseDto();
                }
                products = await productResponse.Content.ReadFromJsonAsync<IEnumerable<Clinic.Data.Models.Product>>().ConfigureAwait(false);
                await this.cacheService.AddOrUpdateCacheAsync<IEnumerable<Clinic.Data.Models.Product>>($"products{filterCriteria}", products).ConfigureAwait(false);
            }


            return new ResponseDto { Result = products.ToList()};
        }

        public async Task<ResponseDto> UpdateProductAsync(Data.Models.Product product)
        {
             using var productRequest = new StringContent(JsonSerializer.Serialize(product), Encoding.UTF8, ContentType);
            var productResponse = await this.httpClient.PutAsync(new Uri($"{this.applicationSettings.Value.DataStoreEndpoint}getproduct"), productRequest).ConfigureAwait(false);
            if (!productResponse.IsSuccessStatusCode)
            {
                await this.ThrowServiceToServiceErrors(productResponse).ConfigureAwait(false);
            }

            // clearning the cache
            await this.cacheService.RemoveCacheAsync("products").ConfigureAwait(false);

            return new ResponseDto();
        }
        private async Task ThrowServiceToServiceErrors(HttpResponseMessage response)
        {
            var exceptionResponse = await response.Content.ReadFromJsonAsync<ExceptionResponse>().ConfigureAwait(false);
            throw new Exception(exceptionResponse.InnerException);
        }
    }
}
