using AutoMapper;
using Clinic.Caching.Interfaces;
using Clinic.Common.Models;
using Clinic.Common.Options;
using Clinic.Common.Validator;
using Clinic.Coupon.Contract;
using Clinic.Data.Models;
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
    private ICouponService couponServiceImplementation;

    public CouponService(IHttpClientFactory httpClientFactory, IOptions<ApplicationSettings> applicationSettings, IMapper autoMapper, IDistributedCacheService cacheService)
    {
        ArgumentValidation.ThrowIfNull(applicationSettings);
        httpClient = httpClientFactory.CreateClient();
        this.autoMapper = autoMapper;
        this.cacheService = cacheService;
        this.applicationSettings = applicationSettings;
    }
    public async Task<ResponseDto> GetCouponByCode(string couponCode)
    {
        CouponDto? coupon = null;
        ResponseDto result = new();
        using var couponRequest = new HttpRequestMessage(HttpMethod.Get,$"{applicationSettings.Value.DataStoreEndpoint}getcoupon?code={couponCode}");
        var couponResponse = await httpClient.SendAsync(couponRequest).ConfigureAwait(false);
        if(!couponResponse.IsSuccessStatusCode){
            await ThrowServiceToServiceErrors(couponResponse).ConfigureAwait(false);
        }
        if(couponResponse.StatusCode != System.Net.HttpStatusCode.NoContent){
            var couponModel = await couponResponse.Content.ReadFromJsonAsync<Clinic.Data.Models.Coupon>().ConfigureAwait(false);
            coupon = autoMapper.Map<CouponDto>(couponModel);
        }
        result.Result = coupon;
        return result;
    }

    public async Task<ResponseDto> GetCoupons(string filterCriteria = null)
    {
        var coupons = await this.cacheService.GetCacheAsync<IEnumerable<Clinic.Data.Models.Coupon>>($"coupons{filterCriteria}").ConfigureAwait(false);
        if(coupons == null)
        {
            using var couponRequest = new HttpRequestMessage(HttpMethod.Get, $"{this.applicationSettings.Value.DataStoreEndpoint}getallcoupons?filterCriteria={filterCriteria}");
            var couponResponse = await this.httpClient.SendAsync(couponRequest).ConfigureAwait(false);
            if(!couponResponse.IsSuccessStatusCode)
            {
                await this.ThrowServiceToServiceErrors(couponResponse).ConfigureAwait(false);
            }
            if(couponResponse.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return new ResponseDto();
            }
                
            coupons = await couponResponse.Content.ReadFromJsonAsync<IEnumerable<Clinic.Data.Models.Coupon>>().ConfigureAwait(false);
            await this.cacheService.AddOrUpdateCacheAsync<IEnumerable<Clinic.Data.Models.Coupon>>($"coupons{filterCriteria}", coupons).ConfigureAwait(false);
        }


        return new ResponseDto { Result = coupons.ToList()};
    }

    public Task<ResponseDto> GetCouponByIdASync(string couponId)
    {
        return couponServiceImplementation.GetCouponByIdASync(couponId);
    }

    public Task<ResponseDto> AddCouponAsync(Data.Models.Coupon coupon)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseDto> UpdateCouponAsync(Data.Models.Coupon coupon)
    {
        throw new NotImplementedException();
    }


    public Task<ResponseDto> DeleteCouponAsync(string couponId)
    {
        return couponServiceImplementation.DeleteCouponAsync(couponId);
    }

    private async Task ThrowServiceToServiceErrors(HttpResponseMessage response)
    {
        var exceptionReponse = await response.Content.ReadFromJsonAsync<ExceptionResponse>().ConfigureAwait(false);
        throw new Exception(exceptionReponse.InnerException);
    }
}