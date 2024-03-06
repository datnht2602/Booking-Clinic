using Clinic.DTO.Models;

namespace Clinic.BlazorWebPWA.Services.IService;

public interface IClinicService : IBaseService
{
    Task<T> GetDoctorsAsync<T>(string filterCriteria = null);
    Task<T> GetBookingViewAsync<T>(string userId);

    Task<T> GetDoctorByIdAsync<T>(string id);

    Task<T> CreateOrUpdateBooking<T>(BookingDetailsViewModel model);

    Task<T> GetBookingByIdAsync<T>(string orderId);

    Task<T> GetInvoiceByIdAsync<T>(string orderId);

    Task<T> SubmitOrder<T>(BookingDetailsViewModel order);
}