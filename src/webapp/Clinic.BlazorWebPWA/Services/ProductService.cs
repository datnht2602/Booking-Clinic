using Clinic.BlazorWebPWA.Services.IService;
using Clinic.Data.Models;
using Clinic.DTO.Models.Dto;
using System.Reflection;

namespace Clinic.BlazorWebPWA.Services
{
    public class ProductService : BaseService, IProductService
    {
        private readonly HttpClient client;
        public ProductService(IHttpClientFactory httpClient) : base(httpClient)
        {
            this.client = httpClient.CreateClient();
        }

        public async Task<T> CreateOrUpdateBooking<T>(Product model, string accessToken)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = ApiType.POST,
                Data = model,
                Url = "getproduct",
                AccessToken = accessToken
            });
        }
        public async Task<T> GetListProducts<T>(string filterCriteria = null)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = ApiType.GET,
                Url = $"getproducts/{filterCriteria}",
                //AccessToken = accessToken,
            });
        }

        public async Task<T> GetProduct<T>(string id)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = ApiType.GET,
                Url = $"getproduct/{id}",
            });
        }
    }
}
