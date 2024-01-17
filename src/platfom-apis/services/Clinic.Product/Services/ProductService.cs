using AutoMapper;
using Clinic.Caching.Interfaces;
using Clinic.Common.Models;
using Clinic.Common.Options;
using Clinic.Data.Models;
using Clinic.DTO.Models;
using Clinic.Product.Contracts;
using Microsoft.Extensions.Options;

namespace Clinic.Product.Services
{
    public class ProductService : IProductService
    {
        private readonly IOptions<ApplicationSettings> applicationSettings;
        private readonly HttpClient httpClient;
        private readonly IMapper autoMapper;
        private readonly IDistributedCacheService cacheService;
        public Task<ProductDetailsViewModel> AddProductAsync(ProductDetailsViewModel product)
        {
            throw new NotImplementedException();
        }

        public Task<HttpResponseMessage> DeleteProductAsync(string productId, string productName)
        {
            throw new NotImplementedException();
        }

        public Task<ProductDetailsViewModel> GetProductByIdASync(string productId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ProductListViewModel>> GetProductsAsync(string filterCriteria = null)
        {
            var products = await this.cacheService.GetCacheAsync<IEnumerable<Clinic.Data.Models.Product>>($"products{filterCriteria}").ConfigureAwait(false);
            if(products == null)
            {
                using var productRequest = new HttpRequestMessage(HttpMethod.Get, $"{this.applicationSettings.Value.DataStoreEndpoint}api/products?filterCriteria={filterCriteria}");
                var productResponse = await this.httpClient.SendAsync(productRequest).ConfigureAwait(false);
                if(!productResponse.IsSuccessStatusCode)
                {
                    await this.ThrowServiceToServiceErrors(productResponse).ConfigureAwait(false);
                }
                if(productResponse.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    return new List<ProductListViewModel>();
                }
                products = await productResponse.Content.ReadFromJsonAsync<IEnumerable<Clinic.Data.Models.Product>>().ConfigureAwait(false);
                await this.cacheService.AddOrUpdateCacheAsync<IEnumerable<Clinic.Data.Models.Product>>($"products{filterCriteria}", products).ConfigureAwait(false);
            }

            var productList = this.autoMapper.Map<List<ProductListViewModel>>(products);
            return productList;
        }

        public Task<HttpResponseMessage> UpdateProductAsync(ProductDetailsViewModel product)
        {
            throw new NotImplementedException();
        }
        private async Task ThrowServiceToServiceErrors(HttpResponseMessage response)
        {
            var exceptionResponse = await response.Content.ReadFromJsonAsync<ExceptionResponse>().ConfigureAwait(false);
            throw new Exception(exceptionResponse.InnerException);
        }
    }
}
