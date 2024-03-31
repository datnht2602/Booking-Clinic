namespace Clinic.BlazorWebPWA.Services.IService;

public interface IDoctorService : IBaseService
{
    Task<T> DeleteDoctor<T>(string id);
    Task<T> GetDoctorById<T>(string id);
}