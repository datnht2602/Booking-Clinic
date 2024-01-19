using AutoMapper;

namespace Clinic.Booking
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            this.MapEntity();
        }
        private void MapEntity() 
        {
            this.CreateMap<Clinic.Data.Models.Booking, Clinic.DTO.Models.BookingDetailsViewModel>();
            this.CreateMap<Data.Models.Product, DTO.Models.ProductListViewModel>();
            this.CreateMap<Data.Models.Brief, DTO.Models.BriefViewModel>();
        }
    }
}
