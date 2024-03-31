using Clinic.BlazorWebPWA.Services.IService;
using Clinic.DTO.Models.Dto;

namespace Clinic.BlazorWebPWA.Services;

public class CouponService : BaseService, ICouponService
{
    private readonly HttpClient client;
    public CouponService(IHttpClientFactory httpClient) : base(httpClient)
    {
        this.client = httpClient.CreateClient();
    }

    public async Task<T> CreateOrUpdateCoupon<T>(CouponDto model, string accessToken)
    {
        return await this.SendAsync<T>(new ApiRequest()
        {
            ApiType = ApiType.POST,
            Data = model,
            Url = "getcoupon",
            AccessToken = accessToken
        });
    }

    public async Task<T> GetListCoupons<T>(string filterCriteria = null)
    {
        return await this.SendAsync<T>(new ApiRequest()
        {
            ApiType = ApiType.GET,
            Url = $"getlistcoupon/{filterCriteria}",
        });
    }

    public async Task<T> GetCoupon<T>(string id)
    {
        return await this.SendAsync<T>(new ApiRequest()
        {
            ApiType = ApiType.GET,
            Url = $"getcoupon/{id}",
        });
    }

    public async Task<T> DeleteCoupon<T>(string id)
    {
        return await this.SendAsync<T>(new ApiRequest()
        {
            ApiType = ApiType.DELETE,
            Url = $"getcoupon/{id}",
        });
    }
}