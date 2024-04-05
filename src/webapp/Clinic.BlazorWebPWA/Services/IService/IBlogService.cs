using Clinic.Data.Models;
using Clinic.DTO.Models;

namespace Clinic.BlazorWebPWA.Services.IService
{
    public interface IBlogService : IBaseService
    {
        Task<T> CreateOrUpdateBlog<T>(Blog model, string accessToken);
        Task<T> GetListBlogs<T>(string filterCriteria = null);
        Task<T> GetBlogById<T>(string id);
        Task<T> DeleteBlog<T>(string id);
    }
}
