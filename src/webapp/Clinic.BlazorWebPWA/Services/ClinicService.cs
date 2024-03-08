using Clinic.BlazorWebPWA.Services.IService;
using Clinic.DTO.Models;
using Clinic.DTO.Models.Dto;

namespace Clinic.BlazorWebPWA.Services;

public class ClinicService :  BaseService,IClinicService
{
    private readonly IHttpClientFactory _clientFactory;
    public ClinicService(IHttpClientFactory httpClient) : base(httpClient)
    {
        _clientFactory = _clientFactory;
    }


    public async Task<T> GetDoctorsAsync<T>(string filterCriteria = null)
    {
        return await this.SendAsync<T>(new ApiRequest()
        {
            ApiType = ApiType.GET,
            Url = "https://localhost:7244/getdoctors",
        });
    }

    public async Task<T> GetBookingViewAsync<T>(string userId,string accessToken)
    {
        return await this.SendAsync<T>(new ApiRequest()
        {
            ApiType = ApiType.GET,
            Url = "https://localhost:7244/getbooking?userId=" + userId,
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
            Url = "https://localhost:7244/upsertbooking",
            AccessToken = accessToken
        });
    }

    public Task<T> GetBookingByIdAsync<T>(string orderId)
    {
        throw new NotImplementedException();
    }

    public Task<T> GetInvoiceByIdAsync<T>(string orderId)
    {
        throw new NotImplementedException();
    }

    public Task<T> SubmitOrder<T>(BookingDetailsViewModel order)
    {
        throw new NotImplementedException();
    }
}