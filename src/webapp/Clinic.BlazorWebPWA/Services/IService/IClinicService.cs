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

    Task<T> GetDetailUser<T>(string userId,string accessToken);

    Task<T> UpdateDetailUser<T>(BriefViewModel model,string id, string accessToken);
    Task<T> GetCoupon<T>(string coupon, string accessToken);
    Task<T> GetFeedBack<T>(string doctorId, string bookingId);
    Task<T> GetInvoice<T>(string bookingId, string accessToken);
    Task<T> GetHealthPackages<T>(string filterCriteria = null);
    Task<T> CreateOrUpdateDoctor<T>(DoctorDto model,string accessToken);
    Task<T> GetListBooking<T>(string id, string accessToken);
    Task<T> SendFeedBack<T>(FormDto form);
    Task<T> GetDetailDoctor<T>(string id);
    Task<T> GetExportString<T>(string id, string accessToken);
    Task<T> ChangePassword<T>(ChangePasswordDto dto, string accessToken);

}