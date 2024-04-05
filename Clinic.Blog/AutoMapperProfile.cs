using AutoMapper;
using Clinic.DTO.Models;
using Clinic.DTO.Models.Model;

namespace Clinic.Product
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() { 
            this.MapEntity();
        }
        private void MapEntity()
        {
            this.CreateMap<Data.Models.Product,HealthPackageModel>().ForMember(des => des.Products,
                act => act.MapFrom(
                    src => src.Products)); ;;
            this.CreateMap<Data.Models.Product, ProductDetailsViewModel>();
            this.CreateMap<Data.Models.Rating, RatingViewModel>();
            this.CreateMap<Data.Models.Product,ProductListViewModel>();
            this.CreateMap<ProductDetailsViewModel, Data.Models.Product>();
        }
    }
}
