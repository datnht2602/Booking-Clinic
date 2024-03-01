using AutoMapper;
using Clinic.Data.Models;
using Clinic.DTO.Models;
using Clinic.DTO.Models.Dto;
using Clinic.Identity.Models;
using Newtonsoft.Json;

namespace Clinic.Identity;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        this.MapEntity();
    }
    private void MapEntity() 
    {
        this.CreateMap<ApplicationUser,ApplicationUsersDto>();
        this.CreateMap<ApplicationUsersDto,ApplicationUserModel>()
                .ForMember(des => des.ExperienceYear,
                           act => act.MapFrom
                           (src => JsonConvert.DeserializeObject<Detail>(src.Detail).ExperienceYear))
                .ForMember(des => des.Title,
                           act => act.MapFrom
                           (src => JsonConvert.DeserializeObject<Detail>(src.Detail).Title ?? string.Empty))
                .ForMember(des => des.Specialization,
                           act => act.MapFrom
                           (src => JsonConvert.DeserializeObject<Detail>(src.Detail).Specialization ))
                .ForMember(des => des.ClinicNum,
                           act => act.MapFrom
                           (src => JsonConvert.DeserializeObject<Detail>(src.Detail).ClinicNum ?? string.Empty));
        this.CreateMap<ApplicationUserModel, DoctorListViewModel>();
    }
}