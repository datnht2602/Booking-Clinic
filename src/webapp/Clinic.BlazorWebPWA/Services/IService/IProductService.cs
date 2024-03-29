using Clinic.Data.Models;
using Clinic.DTO.Models;

namespace Clinic.BlazorWebPWA.Services.IService
{
    public interface IProductService : IBaseService
    {
        Task<T> CreateOrUpdateBooking<T>(Product model, string accessToken);
        Task<T> GetListProducts<T>(string filterCriteria = null);
        Task<T> GetProduct<T>(string id);
    }
}
