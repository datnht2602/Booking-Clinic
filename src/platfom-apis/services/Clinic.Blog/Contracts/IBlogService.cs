using Clinic.DTO.Models.Dto;

namespace Clinic.Blog.Contracts
{
    public interface IBlogService
    {
        Task<ResponseDto> GetBlogsAsync(string filterCriteria = null);
        Task<ResponseDto> GetBlogByIdASync(string id);
        Task<ResponseDto> AddBlogAsync(Data.Models.Blog blog);
        Task<ResponseDto> UpdateBlogAsync(Data.Models.Blog product);
        Task<ResponseDto> DeleteBlogAsync(string id);
    }
}
