using AutoMapper;
using Clinic.Caching.Interfaces;
using Clinic.Common.Models;
using Clinic.Common.Options;
using Clinic.Common.Validator;
using Clinic.Coupon.Contract;
using Clinic.DTO.Models.Dto;
using Microsoft.Extensions.Options;

namespace Clinic.Coupon.Services;

public class CouponService : ICouponService
{
    private const string ContentType = "application/json";
    private readonly IOptions<ApplicationSettings> applicationSettings;
    private readonly HttpClient httpClient;
    private readonly IMapper autoMapper;
    private readonly IDistributedCacheService cacheService;

    public CouponService(IHttpClientFactory httpClientFactory, IOptions<ApplicationSettings> applicationSettings, IMapper autoMapper, IDistributedCacheService cacheService)
    {
        ArgumentValidation.ThrowIfNull(applicationSettings);
        httpClient = httpClientFactory.CreateClient();
        this.autoMapper = autoMapper;
        this.cacheService = cacheService;
        this.applicationSettings = applicationSettings;
    }
    public async Task<CouponDto> GetCouponByCode(string couponCode)
    {
        CouponDto? coupon = null;
        using var couponRequest = new HttpRequestMessage(HttpMethod.Get,$"{applicationSettings.Value.DataStoreEndpoint}getcoupon/{couponCode}");
        var couponResponse = await httpClient.SendAsync(couponRequest).ConfigureAwait(false);
        if(!couponResponse.IsSuccessStatusCode){
            await ThrowServiceToServiceErrors(couponResponse).ConfigureAwait(false);
        }
        if(couponResponse.StatusCode != System.Net.HttpStatusCode.NoContent){
            var couponModel = await couponResponse.Content.ReadFromJsonAsync<Clinic.Data.Models.Coupon>().ConfigureAwait(false);
            coupon = autoMapper.Map<CouponDto>(couponModel);
        }
        return coupon;
    }
    private async Task ThrowServiceToServiceErrors(HttpResponseMessage response)
    {
        var exceptionReponse = await response.Content.ReadFromJsonAsync<ExceptionResponse>().ConfigureAwait(false);
        throw new Exception(exceptionReponse.InnerException);
    }
}