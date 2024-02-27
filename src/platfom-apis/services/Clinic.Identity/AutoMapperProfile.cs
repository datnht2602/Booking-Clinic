using AutoMapper;
using Clinic.DTO.Models;
using Clinic.DTO.Models.Dto;
using Clinic.Identity.Models;

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
        this.CreateMap<ApplicationUsersDto,ApplicationUserModel>();
    }
}