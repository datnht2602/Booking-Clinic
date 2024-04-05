using Clinic.BlazorWebPWA.Services.IService;
using Clinic.Data.Models;
using Clinic.DTO.Models.Dto;

namespace Clinic.BlazorWebPWA.Services
{
    public class BlogService : BaseService, IBlogService
    {
        private readonly HttpClient client;
        public BlogService(IHttpClientFactory httpClient) : base(httpClient)
        {
            this.client = httpClient.CreateClient();
        }

        public async Task<T> CreateOrUpdateBlog<T>(Blog model, string accessToken)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = ApiType.POST,
                Data = model,
                Url = "getblog",
                AccessToken = accessToken
            });
        }

        public async Task<T> DeleteBlog<T>(string id)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = ApiType.DELETE,
                Url = $"getblog/{id}",
            });
        }

        public async Task<T> GetBlogById<T>(string id)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = ApiType.GET,
                Url = $"getblog/{id}",
            });
        }

        public async Task<T> GetListBlogs<T>(string filterCriteria = null)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = ApiType.GET,
                Url = $"getblogs/{filterCriteria}",
                //AccessToken = accessToken,
            });
        }
    }
}
