using AutoMapper;
using Clinic.DTO.Models;

namespace Clinic.Product
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() { 
            this.MapEntity();
        }
        private void MapEntity()
        {
            this.CreateMap<Data.Models.Product, ProductDetailsViewModel>();
            this.CreateMap<Data.Models.Rating, RatingViewModel>();
            this.CreateMap<Data.Models.Product,ProductListViewModel>();
            this.CreateMap<ProductDetailsViewModel, Data.Models.Product>();
        }
    }
}
