using Clinic.BlazorWebPWA.Services.IService;
using Clinic.Common.Options;
using Clinic.DTO.Models;
using Clinic.DTO.Models.Dto;
using Microsoft.Extensions.Options;

namespace Clinic.BlazorWebPWA.Services;

public class ClinicService :  BaseService,IClinicService
{
    private readonly IOptions<ApplicationSettings> applicationSettings;
    private readonly HttpClient client;
    public ClinicService(IHttpClientFactory httpClient,IOptions<ApplicationSettings> applicationSettings) : base(httpClient)
    {
        this.applicationSettings = applicationSettings;
        this.client = httpClient.CreateClient("ApiGateway");
    }


    public async Task<T> GetDoctorsAsync<T>(string filterCriteria = null)
    {
        return await this.SendAsync<T>(new ApiRequest()
        {
            ApiType = ApiType.GET,
            Url = $"getdoctors",
        });
    }

    public async Task<T> GetBookingViewAsync<T>(string userId,string accessToken)
    {
        return await this.SendAsync<T>(new ApiRequest()
        {
            ApiType = ApiType.GET,
            Url = "getbooking?userId=" + userId,
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
            Url = "upsertbooking",
            AccessToken = accessToken
        });
    }

    public async Task<T> GetBookingByIdAsync<T>(string bookingId, string accessToken)
    {
        return await this.SendAsync<T>(new ApiRequest()
        {
            ApiType = ApiType.GET,
            Url = $"getbookingbyid?bookingId={bookingId}",
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
            Url = $"coupon/{coupon}",
            AccessToken = accessToken
        });
    }
}