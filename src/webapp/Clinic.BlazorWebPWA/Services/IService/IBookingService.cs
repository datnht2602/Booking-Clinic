using Clinic.Data.Models;
using Clinic.DTO.Models;

namespace Clinic.BlazorWebPWA.Services.IService
{
    public interface IBookingService : IBaseService
    {
        Task<T> CreateOrUpdateBooking<T>(BookingDetailsViewModel model, string accessToken);
        Task<T> GetListBookings<T>(string filterCriteria = null);
        Task<T> GetBookingById<T>(string id);
        Task<T> DeleteBooking<T>(string id);
    }
}
