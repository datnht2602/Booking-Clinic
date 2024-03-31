using Clinic.DTO.Models.Dto;

namespace Clinic.Coupon.Contract;

public interface ICouponService
{
    Task<ResponseDto> GetCouponByCode(string couponCode);
    Task<ResponseDto> GetCoupons(string filterCriteria = null);
    Task<ResponseDto> GetCouponByIdASync(string coupon);
    Task<ResponseDto> AddCouponAsync(Data.Models.Coupon coupon);
    Task<ResponseDto> UpdateCouponAsync(Data.Models.Coupon coupon);
    Task<ResponseDto> DeleteCouponAsync(string couponId);
}