using Clinic.BlazorWebPWA.Pages;
using Clinic.BlazorWebPWA.Services.IService;
using Clinic.Common.Options;
using Clinic.DTO.Models;
using Clinic.DTO.Models.Dto;
using Clinic.Message;
using Microsoft.Extensions.Options;
using Net.payOS;
using System.Reflection;

namespace Clinic.BlazorWebPWA.Services;

public class ClinicService :  BaseService,IClinicService
{
    private readonly IOptions<ApplicationSettings> applicationSettings;
    private readonly HttpClient client;
    private readonly PayOS _payOS;

    public ClinicService(IHttpClientFactory httpClient,IOptions<ApplicationSettings> applicationSettings,PayOS payOS) : base(httpClient)
    {
        this.applicationSettings = applicationSettings;
        this.client = httpClient.CreateClient("ApiGateway");
        _payOS = payOS;

    }


    public async Task<T> GetDoctorsAsync<T>(FilterDto dto)
    {
        return await this.SendAsync<T>(new ApiRequest()
        {
            ApiType = ApiType.POST,
            Url = $"getdoctors",
            Data = dto,
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

    public async Task<T> GetDetailUser<T>(string userId, string accessToken)
    {
        return await this.SendAsync<T>(new ApiRequest()
        {
            ApiType = ApiType.GET,
            Url = $"getdetailuser/{userId}",
            AccessToken = accessToken
        });
    }
    

    public async Task<T> UpdateDetailUser<T>(BriefViewModel model,string id, string accessToken)
    {
        return await this.SendAsync<T>(new ApiRequest()
        {
            ApiType = ApiType.POST,
            Url = $"updatedetailuser/{id}",
            AccessToken = accessToken,
            Data = model
        });
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

    public async Task<T> GetFeedBack<T>(string doctorId, string bookingId)
    {
        return await this.SendAsync<T>(new ApiRequest()
        {
            ApiType = ApiType.GET,
            Url = $"feedback/{doctorId}/{bookingId}"
        });
    }
    

    public async Task<T> GetInvoice<T>(string bookingId, string accessToken)
    {
        return await this.SendAsync<T>(new ApiRequest()
        {
            ApiType = ApiType.GET,
            Url = $"getinvoice/{bookingId}",
            AccessToken = accessToken
        });
    }
    public async Task<T> GetHealthPackages<T>(string filterCriteria = null)
    {
        return await this.SendAsync<T>(new ApiRequest()
        {
            ApiType = ApiType.GET,
            Url = $"gethealthpackge/{filterCriteria}",
        });
    }

    public async Task<T> CreateOrUpdateDoctor<T>(DoctorDto model, string accessToken)
    {
        return await this.SendAsync<T>(new ApiRequest()
        {
            ApiType = ApiType.POST,
            Url = $"createdoctor",
            //AccessToken = accessToken,
            Data = model
        });
    }

    public async Task<T> GetListBooking<T>(string id, string accessToken)
    {
        return await this.SendAsync<T>(new ApiRequest()
        {
            ApiType = ApiType.POST,
            Url = $"getlistbooking/{id}",
            AccessToken = accessToken,
        });
    }

    public async Task<T> SendFeedBack<T>(FormDto form)
    {
        return await this.SendAsync<T>(new ApiRequest()
        {
            ApiType = ApiType.POST,
            Url = $"sendfeedback",
            Data = form
        });
    }

    public async Task<T> GetDetailDoctor<T>(string id)
    {
        return await this.SendAsync<T>(new ApiRequest()
        {
            ApiType = ApiType.GET,
            Url = $"getdetaildoctor/{id}",
        });
    }

    public async Task<T> GetScheduleAsync<T>(string doctorId)
    {
        return await this.SendAsync<T>(new ApiRequest()
        {
            ApiType = ApiType.GET,
            Url = $"getschedule/{doctorId}",
            //AccessToken = accessToken,
        });
    }
    public async Task<T> GetExportString<T>(string bookingId, string accessToken)
    {
        return await this.SendAsync<T>(new ApiRequest()
        {
            ApiType = ApiType.GET,
            Url = $"getexportfile/{bookingId}",
            AccessToken = accessToken,
        });
    }
    public async Task<T> ChangePassword<T>(ChangePasswordDto dto, string accessToken)
    {
        return await this.SendAsync<T>(new ApiRequest()
        {
            ApiType = ApiType.POST,
            Url = "changepassword",
            AccessToken = accessToken,
            Data = dto
        });
    }

    public async Task<T> ChangeSchedule<T>(BookingDetailsViewModel model, string accessToken)
    {
        return await this.SendAsync<T>(new ApiRequest()
        {
            ApiType = ApiType.POST,
            Url = "changeschedule",
            AccessToken = accessToken,
            Data = model
        });
    }

    public async Task<T> ChangeDoctor<T>(BookingDetailsViewModel dto, string accessToken)
    {
        return await this.SendAsync<T>(new ApiRequest()
        {
            ApiType = ApiType.POST,
            Url = "changedoctor",
            AccessToken = accessToken,
            Data = dto
        });
    }
}