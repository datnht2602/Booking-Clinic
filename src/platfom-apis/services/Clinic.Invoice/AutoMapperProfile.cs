using AutoMapper;
using Clinic.Data.Models;
using Newtonsoft.Json;

namespace Clinic.Invoice
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() 
        {
            this.MapEntity();
        }
        private void MapEntity()
        {
            this.CreateMap<Data.Models.Invoice, DTO.Models.InvoiceDetailsViewModel>()
                .ForMember(des => des.BriefViewModel,
                           act => act.MapFrom
                           (src => src.AdditionalData))
                .ForMember(des => des.Products,
                           act => act.MapFrom
                           (src => src.Product))
                .ForMember(des => des.Detail,
                           act => act.MapFrom
                           (src => src.Doctor));
            this.CreateMap<DTO.Models.InvoiceDetailsViewModel, Data.Models.Invoice>()
                .ForMember(des => des.AdditionalData,
                           act => act.MapFrom
                           (src => src.BriefViewModel))
                .ForMember(des => des.Product,
                           act => act.MapFrom
                           (src => src.Products))
                .ForMember(des => des.Doctor,
                           act => act.MapFrom
                           (src => src.Detail)); 
            this.CreateMap<Data.Models.Product, DTO.Models.ProductListViewModel>();
                this.CreateMap<DTO.Models.ProductListViewModel, Data.Models.Product>();
            this.CreateMap<Data.Models.AddtionalData, DTO.Models.BriefViewModel>();
            this.CreateMap<DTO.Models.BriefViewModel, Data.Models.AddtionalData>();
            this.CreateMap<Data.Models.DoctorInfo, Data.Models.DetailDto>();
            this.CreateMap<Data.Models.DetailDto, Data.Models.DoctorInfo>();
        }
    }
}
