using Clinic.DTO.Models.Dto;

namespace Clinic.Product.Contracts
{
    public interface IProductService
    {
        Task<ResponseDto> GetProductsAsync(string filterCriteria = null);
        Task<ResponseDto> GetProductByIdASync(string productId);
        Task<ResponseDto> AddProductAsync(Data.Models.Product product);
        Task<ResponseDto> UpdateProductAsync(Data.Models.Product product);
        Task<ResponseDto> DeleteProductAsync(string productId);
    }
}
