using AutoMapper;
using Clinic.DTO.Models.Dto;

namespace Clinic.Coupon;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        this.MapEntity();
    }
    private void MapEntity() 
    {
        this.CreateMap<Clinic.Data.Models.Coupon, CouponDto>();
    }
}