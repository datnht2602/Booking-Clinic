using Clinic.DTO.Models.Dto;

namespace Clinic.Coupon.Contract;

public interface ICouponService
{
    Task<CouponDto> GetCouponByCode(string couponCode);
}