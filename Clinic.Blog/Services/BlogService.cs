using System.Net.Mime;
using System.Text;
using System.Text.Json;
using AutoMapper;
using Clinic.Blog.Contracts;
using Clinic.Caching.Interfaces;
using Clinic.Common.Models;
using Clinic.Common.Options;
using Clinic.Common.Validator;
using Clinic.Data.Models;
using Clinic.DTO.Models;
using Clinic.DTO.Models.Dto;
using Clinic.DTO.Models.Model;
using Microsoft.Extensions.Options;

namespace Clinic.Blog.Services
{
    public class BlogService : IBlogService
    {
        private const string ContentType = "application/json";
        private readonly IOptions<ApplicationSettings> applicationSettings;
        private readonly HttpClient httpClient;
        private readonly IMapper autoMapper;
        private readonly IDistributedCacheService cacheService;
        public BlogService(IHttpClientFactory httpClientFactory, IOptions<ApplicationSettings> applicationSettings,
        IMapper autoMapper, IDistributedCacheService cacheService)
        {
            ArgumentNullException.ThrowIfNull(applicationSettings,nameof(applicationSettings));
            this.applicationSettings = applicationSettings;
            this.httpClient = httpClientFactory.CreateClient();
            this.autoMapper = autoMapper;
            this.cacheService = cacheService;
        }

        public async Task<ResponseDto> AddBlogAsync(Data.Models.Blog blog)
        {
            ArgumentValidation.ThrowIfNull(blog);
            ResponseDto result = new();
            var existingProduct = await GetBlogByIdASync(blog.Id);
            if (existingProduct != null && existingProduct.IsSuccess)
            {
                return await UpdateBlogAsync(blog);
            }
            blog.Id = Guid.NewGuid().ToString();
            blog.CreatedDate = DateTime.Now;
            using var blogRequest = new StringContent(JsonSerializer.Serialize(blog), Encoding.UTF8, ContentType);
            var blogResponse = await httpClient.PostAsync(new Uri($"{this.applicationSettings.Value.DataStoreEndpoint}getblog"), blogRequest).ConfigureAwait(false);

            if (!blogResponse.IsSuccessStatusCode)
            {
                await ThrowServiceToServiceErrors(blogResponse).ConfigureAwait(false);
            }
            var createdBlogDAO = await blogResponse.Content.ReadFromJsonAsync<Data.Models.Blog>().ConfigureAwait(false);

            await cacheService.RemoveCacheAsync($"blogs").ConfigureAwait(false);
            if (createdBlogDAO != null)
            {
                result.Result = createdBlogDAO;
                return result;
            }

            result.IsSuccess = false;
            return result;
        }

        public async Task<ResponseDto> DeleteBlogAsync(string id)
        {
            var blogResponse = await httpClient.DeleteAsync(new Uri($"{this.applicationSettings.Value.DataStoreEndpoint}getblog/{id}")).ConfigureAwait(false);
            if (!blogResponse.IsSuccessStatusCode)
            {
                await this.ThrowServiceToServiceErrors(blogResponse).ConfigureAwait(false);
            }
            await cacheService.RemoveCacheAsync($"blogs").ConfigureAwait(false);
            return new ResponseDto();
        }

        public async Task<ResponseDto> GetBlogByIdASync(string id)
        {
            if (id == null)
            {
                return null;
            }
            using var blogRequest = new HttpRequestMessage(HttpMethod.Get, $"{this.applicationSettings.Value.DataStoreEndpoint}getblog/{id}");
            var blogResponse = await httpClient.SendAsync(blogRequest).ConfigureAwait(false);
            ResponseDto result = new();
            if (!blogResponse.IsSuccessStatusCode)
            {
                await ThrowServiceToServiceErrors(blogResponse).ConfigureAwait(false);
            }
            if (blogResponse.StatusCode != System.Net.HttpStatusCode.NoContent)
            {
                var blogDAO = await blogResponse.Content.ReadFromJsonAsync<Clinic.Data.Models.Blog>().ConfigureAwait(false);
                result.Result = blogDAO;
                return result;
            }
            else
            {
                result.IsSuccess = false;
                return result;
            }
        }

        public async Task<ResponseDto> GetBlogsAsync(string filterCriteria = null)
        {
            var blogs = await this.cacheService.GetCacheAsync<IEnumerable<Clinic.Data.Models.Blog>>($"blogs").ConfigureAwait(false);
            if (blogs == null)
            {
                using var blogRequest = new HttpRequestMessage(HttpMethod.Get, $"{this.applicationSettings.Value.DataStoreEndpoint}getblogs?filterCriteria={filterCriteria}");
                var blogResponse = await this.httpClient.SendAsync(blogRequest).ConfigureAwait(false);
                if (!blogResponse.IsSuccessStatusCode)
                {
                    await this.ThrowServiceToServiceErrors(blogResponse).ConfigureAwait(false);
                }
                if (blogResponse.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    return new ResponseDto();
                }

                blogs = await blogResponse.Content.ReadFromJsonAsync<IEnumerable<Clinic.Data.Models.Blog>>().ConfigureAwait(false);
                await this.cacheService.AddOrUpdateCacheAsync<IEnumerable<Clinic.Data.Models.Blog>>($"blogs", blogs).ConfigureAwait(false);
            }


            return new ResponseDto { Result = blogs.ToList() };
        }

        public async Task<ResponseDto> UpdateBlogAsync(Data.Models.Blog blog)
        {
            using var blogRequest = new StringContent(JsonSerializer.Serialize(blog), Encoding.UTF8, ContentType);
            var blogResponse = await this.httpClient.PutAsync(new Uri($"{this.applicationSettings.Value.DataStoreEndpoint}getblog"), blogRequest).ConfigureAwait(false);
            if (!blogResponse.IsSuccessStatusCode)
            {
                await this.ThrowServiceToServiceErrors(blogResponse).ConfigureAwait(false);
            }

            // clearning the cache
            await cacheService.RemoveCacheAsync($"blogs").ConfigureAwait(false);
            return new ResponseDto();
        }

        private async Task ThrowServiceToServiceErrors(HttpResponseMessage response)
        {
            var exceptionResponse = await response.Content.ReadFromJsonAsync<ExceptionResponse>().ConfigureAwait(false);
            throw new Exception(exceptionResponse.InnerException);
        }
    }
}
