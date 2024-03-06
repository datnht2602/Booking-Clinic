using Clinic.DTO.Models.Dto;

namespace Clinic.BlazorWebPWA.Services.IService;

public interface IBaseService : IDisposable
{
    
        ResponseDto responseModel { get; set; }
        Task<T> SendAsync<T>(ApiRequest apiRequest);
}