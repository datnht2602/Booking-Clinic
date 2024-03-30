using Clinic.DTO.Models;
using Clinic.DTO.Models.Dto;

namespace Clinic.BlazorWebPWA.Services.IService;

public interface IClinicService : IBaseService
{
    Task<T> GetDoctorsAsync<T>(FilterDto dto);
    Task<T> GetScheduleAsync<T>(string doctorId);
    Task<T> GetBookingViewAsync<T>(string userId,string accessToken);

    Task<T> GetDoctorByIdAsync<T>(string id);

    Task<T> CreateOrUpdateBooking<T>(BookingDetailsViewModel model,string accessToken);

    Task<T> GetBookingByIdAsync<T>(string bookingId, string accessToken);

    Task<T> GetInvoiceByIdAsync<T>(string orderId);

    Task<T> SubmitOrder<T>(BookingDetailsViewModel order);
    Task<T> GetCoupon<T>(string coupon, string accessToken);
    Task<T> ChangeBookingStatus<T>(string bookingId, string accessToken);
    Task<T> GetInvoice<T>(string bookingId, string accessToken);
    Task<T> GetHealthPackages<T>(string filterCriteria = null);
    Task<T> GetListDoctors<T>(DoctorDto model, string accessToken);
    Task<T> GetDoctor<T>(string id);
    Task<T> CreateOrUpdateDoctor<T>(DoctorDto model,string accessToken);
    Task<T> DeleteDoctor<T>(string id);

}