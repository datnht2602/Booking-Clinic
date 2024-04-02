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
                .ForMember(des => des.SoldBy,
                           act => act.MapFrom
                           (src => src.SoldBy));
            this.CreateMap<DTO.Models.InvoiceDetailsViewModel, Data.Models.Invoice>()
                .ForMember(des => des.AdditionalData,
                           act => act.MapFrom
                           (src => src.BriefViewModel))
                .ForMember(des => des.Product,
                           act => act.MapFrom
                           (src => src.Products))
                .ForMember(des => des.SoldBy,
                           act => act.MapFrom
                           (src => src.SoldBy)); 
            this.CreateMap<Data.Models.Product, DTO.Models.ProductListViewModel>();
                this.CreateMap<DTO.Models.ProductListViewModel, Data.Models.Product>();
            this.CreateMap<Data.Models.AddtionalData, DTO.Models.BriefViewModel>();
            this.CreateMap<DTO.Models.BriefViewModel, Data.Models.AddtionalData>();
            this.CreateMap<Data.Models.SoldBy, DTO.Models.SoldByViewModel>();
            this.CreateMap<DTO.Models.SoldByViewModel, Data.Models.SoldBy>();
        }
    }
}
