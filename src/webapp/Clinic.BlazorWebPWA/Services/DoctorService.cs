using Clinic.BlazorWebPWA.Services.IService;
using Clinic.DTO.Models.Dto;

namespace Clinic.BlazorWebPWA.Services;

public class DoctorService : BaseService, IDoctorService
{
    private readonly HttpClient client;
    public DoctorService(IHttpClientFactory httpClient) : base(httpClient)
    {
        this.client = httpClient.CreateClient();
    }

    public async Task<T> DeleteDoctor<T>(string id)
    {
        return await this.SendAsync<T>(new ApiRequest()
        {
            ApiType = ApiType.DELETE,
            Url = $"deletedoctor/{id}",
        });
    }

    public async Task<T> GetDoctorById<T>(string id)
    {
        return await this.SendAsync<T>(new ApiRequest()
        {
            ApiType = ApiType.GET,
            Url = $"getdoctor/{id}",
        });
    }
}