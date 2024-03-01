using Clinic.DTO.Models.Dto;
using Clinic.DTO.Models;
using AutoMapper;
using Newtonsoft.Json;
using Clinic.Data.Models;

namespace Clinic.ApiGateway
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            this.MapEntity();
        }
        private void MapEntity()
        {
            this.CreateMap<ApplicationUserModel, DoctorListViewModel>();
        }
    }
}
