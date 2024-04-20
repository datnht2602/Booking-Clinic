using AutoMapper;
using Clinic.Caching.Interfaces;
using Clinic.Common.Models;
using Clinic.Common.Options;
using Clinic.Common.Validator;
using Clinic.Coupon.Contract;
using Clinic.Data.Models;
using Clinic.DTO.Models.Dto;
using Microsoft.Extensions.Options;
using System.Text;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

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
            couponModel.Quantity -= 1;
            if (couponModel.Quantity <= 0)
            {
                couponModel.IsEnable = false;
            }
            coupon = autoMapper.Map<CouponDto>(couponModel);
            await this.UpdateCouponAsync(couponModel);
        }
        result.Result = coupon;
        return result;
    }

    public async Task<ResponseDto> GetCoupons(string filterCriteria = null)
    {
        var coupons = await this.cacheService.GetCacheAsync<IEnumerable<Clinic.Data.Models.Coupon>>($"coupons").ConfigureAwait(false);
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
        }


        return new ResponseDto { Result = coupons.ToList()};
    }

    public async Task<ResponseDto> GetCouponByIdASync(string couponId)
    {
        if (couponId == null)
        {
            return null;
        }
        using var couponRequest = new HttpRequestMessage(HttpMethod.Get, $"{this.applicationSettings.Value.DataStoreEndpoint}getcoupons/{couponId}");
        var couponResponse = await httpClient.SendAsync(couponRequest).ConfigureAwait(false);
        ResponseDto result = new();
        if (!couponResponse.IsSuccessStatusCode)
        {
            await ThrowServiceToServiceErrors(couponResponse).ConfigureAwait(false);
        }
        if (couponResponse.StatusCode != System.Net.HttpStatusCode.NoContent)
        {
            var productDAO = await couponResponse.Content.ReadFromJsonAsync<Clinic.Data.Models.Coupon>().ConfigureAwait(false);
            result.Result = productDAO;
            return result;
        }
        else
        {
            return result;
        }
    }

    public async Task<ResponseDto> AddCouponAsync(Data.Models.Coupon coupon)
    {
        ArgumentValidation.ThrowIfNull(coupon);
        ResponseDto result = new();
        var existingProduct = await GetCouponByIdASync(couponId: coupon.Id);
        if (existingProduct != null && existingProduct.IsSuccess)
        {
            return await UpdateCouponAsync(coupon);
        }
        coupon.Id = Guid.NewGuid().ToString();
        using var couponRequest = new StringContent(JsonSerializer.Serialize(coupon), Encoding.UTF8, ContentType);
        var couponResponse = await httpClient.PostAsync(new Uri($"{this.applicationSettings.Value.DataStoreEndpoint}getcoupons"), couponRequest).ConfigureAwait(false);

        if (!couponResponse.IsSuccessStatusCode)
        {
            await ThrowServiceToServiceErrors(couponResponse).ConfigureAwait(false);
        }
        var createdCouponDAO = await couponResponse.Content.ReadFromJsonAsync<Data.Models.Coupon>().ConfigureAwait(false);
        await cacheService.RemoveCacheAsync($"coupons").ConfigureAwait(false);
        if (createdCouponDAO != null)
        {
            result.Result = createdCouponDAO;
            return result;
        }

        result.IsSuccess = false;
        return result;
    }

    public async Task<ResponseDto> UpdateCouponAsync(Data.Models.Coupon coupon)
    {
        using var couponRequest = new StringContent(JsonSerializer.Serialize(coupon), Encoding.UTF8, ContentType);
        var couponResponse = await this.httpClient.PutAsync(new Uri($"{this.applicationSettings.Value.DataStoreEndpoint}getcoupons"), couponRequest).ConfigureAwait(false);
        if (!couponResponse.IsSuccessStatusCode)
        {
            await this.ThrowServiceToServiceErrors(couponResponse).ConfigureAwait(false);
        }

        // clearning the cache
        return new ResponseDto();
    }


    public async Task<ResponseDto> DeleteCouponAsync(string couponId)
    {
        var couponResponse = await httpClient.DeleteAsync(new Uri($"{this.applicationSettings.Value.DataStoreEndpoint}getcoupons/{couponId}")).ConfigureAwait(false);
        if (!couponResponse.IsSuccessStatusCode)
        {
            await this.ThrowServiceToServiceErrors(couponResponse).ConfigureAwait(false);
        }
        await cacheService.RemoveCacheAsync($"coupons").ConfigureAwait(false);
        return new ResponseDto();
    }

    public async Task<ResponseDto> RemoveCoupon(string couponId)
    {
        ResponseDto result = new();
        if (couponId == null)
        {
            return null;
        }
        using var couponRequest = new HttpRequestMessage(HttpMethod.Get, $"{this.applicationSettings.Value.DataStoreEndpoint}getcoupons/{couponId}");
        var couponResponse = await httpClient.SendAsync(couponRequest).ConfigureAwait(false);
        if (!couponResponse.IsSuccessStatusCode)
        {
            await ThrowServiceToServiceErrors(couponResponse).ConfigureAwait(false);
        }
        if (couponResponse.StatusCode != System.Net.HttpStatusCode.NoContent)
        {
            var coupon = await couponResponse.Content.ReadFromJsonAsync<Clinic.Data.Models.Coupon>().ConfigureAwait(false);
            coupon.Quantity += 1;
            coupon.IsEnable = true;
            await UpdateCouponAsync(coupon);
            return result;
        }
        result.IsSuccess = false;
        return result;
    }

    private async Task ThrowServiceToServiceErrors(HttpResponseMessage response)
    {
        var exceptionReponse = await response.Content.ReadFromJsonAsync<ExceptionResponse>().ConfigureAwait(false);
        throw new Exception(exceptionReponse.InnerException);
    }
}