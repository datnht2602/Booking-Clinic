using AutoMapper;

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
            this.CreateMap<Data.Models.Invoice, DTO.Models.InvoiceDetailsViewModel>();
            this.CreateMap<Data.Models.Product, DTO.Models.ProductListViewModel>();
            this.CreateMap<Data.Models.AddtionalData, DTO.Models.BriefViewModel>();
            this.CreateMap<Data.Models.SoldBy, DTO.Models.SoldByViewModel>();
        }
    }
}
