﻿using AutoMapper;
using Clinic.DTO.Models.Message;

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
            this.CreateMap<Clinic.Data.Models.Booking, Clinic.DTO.Models.BookingDetailsViewModel>()
                 .ForMember(des => des.BriefViewModel,
                    act => act.MapFrom(
                        src => src.AdditionalData)); ;
            this.CreateMap<Clinic.DTO.Models.BookingDetailsViewModel, Clinic.Data.Models.Booking>()
                .ForMember(des => des.AdditionalData,
                    act => act.MapFrom(
                        src => src.BriefViewModel))
                .ForMember(des => des.InvoiceId,
                    act => act.MapFrom(
                        src => src.InvoiceId))
                .ForMember(des => des.ClinicNum,
                    act => act.MapFrom(
                        src => src.ClinicNum));
            this.CreateMap<Clinic.DTO.Models.BookingDetailsViewModel, BookingDetailDto>()
                .ForMember(des => des.BookingId,
                    act => act.MapFrom(
                        src => src.Id))
                .ForMember(des => des.BriefViewModel,
                    act => act.MapFrom(
                        src => src.BriefViewModel));
            this.CreateMap<Data.Models.Product, DTO.Models.ProductListViewModel>();
            this.CreateMap<DTO.Models.ProductListViewModel, Data.Models.Product>();
            this.CreateMap<Data.Models.AddtionalData, DTO.Models.BriefViewModel>();
            this.CreateMap< DTO.Models.BriefViewModel, Data.Models.AddtionalData>();
        }
    }
}
