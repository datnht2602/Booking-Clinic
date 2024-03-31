using Clinic.DTO.Models.Dto;

namespace Clinic.BlazorWebPWA.Services.IService;

public interface ICouponService : IBaseService
{
    Task<T> CreateOrUpdateCoupon<T>(CouponDto model, string accessToken);
    Task<T> GetListCoupons<T>(string filterCriteria = null);
    Task<T> GetCoupon<T>(string id);
    Task<T> DeleteCoupon<T>(string id);
}