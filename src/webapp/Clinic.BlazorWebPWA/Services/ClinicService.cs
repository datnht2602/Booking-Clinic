using Clinic.BlazorWebPWA.Services.IService;
using Clinic.Common.Options;
using Clinic.DTO.Models;
using Clinic.DTO.Models.Dto;
using Microsoft.Extensions.Options;

namespace Clinic.BlazorWebPWA.Services;

public class ClinicService :  BaseService,IClinicService
{
    private readonly IOptions<ApplicationSettings> applicationSettings;
    public ClinicService(IHttpClientFactory httpClient,IOptions<ApplicationSettings> applicationSettings) : base(httpClient)
    {
        this.applicationSettings = applicationSettings;
    }


    public async Task<T> GetDoctorsAsync<T>(string filterCriteria = null)
    {
        return await this.SendAsync<T>(new ApiRequest()
        {
            ApiType = ApiType.GET,
            Url = $"{applicationSettings.Value.ApiGatewayEndpoint}/getdoctors",
        });
    }

    public async Task<T> GetBookingViewAsync<T>(string userId,string accessToken)
    {
        return await this.SendAsync<T>(new ApiRequest()
        {
            ApiType = ApiType.GET,
            Url = $"{applicationSettings.Value.ApiGatewayEndpoint}/getbooking?userId=" + userId,
            AccessToken = accessToken
        });
    }

    public Task<T> GetDoctorByIdAsync<T>(string id)
    {
        throw new NotImplementedException();
    }

    public async Task<T> CreateOrUpdateBooking<T>(BookingDetailsViewModel model,string accessToken)
    {
        return await this.SendAsync<T>(new ApiRequest()
        {
            ApiType = ApiType.POST,
            Data = model,
            Url = $"{applicationSettings.Value.ApiGatewayEndpoint}/upsertbooking",
            AccessToken = accessToken
        });
    }

    public async Task<T> GetBookingByIdAsync<T>(string bookingId, string accessToken)
    {
        return await this.SendAsync<T>(new ApiRequest()
        {
            ApiType = ApiType.GET,
            Url = $"{applicationSettings.Value.ApiGatewayEndpoint}/getbookingbyid?bookingId={bookingId}",
            AccessToken = accessToken
        });
    }

    public Task<T> GetInvoiceByIdAsync<T>(string orderId)
    {
        throw new NotImplementedException();
    }

    public Task<T> SubmitOrder<T>(BookingDetailsViewModel order)
    {
        throw new NotImplementedException();
    }

    public async Task<T> GetCoupon<T>(string coupon, string accessToken)
    {
        return await this.SendAsync<T>(new ApiRequest()
        {
            ApiType = ApiType.GET,
            Url = $"{applicationSettings.Value.ApiGatewayEndpoint}/coupon/{coupon}",
            AccessToken = accessToken
        });
    }
}