using Clinic.BlazorWebPWA.Services.IService;
using Clinic.DTO.Models;
using Clinic.DTO.Models.Dto;

namespace Clinic.BlazorWebPWA.Services
{
    public class BookingService : BaseService, IBookingService
    {
        private readonly HttpClient client;
        public BookingService(IHttpClientFactory httpClient) : base(httpClient)
        {
            this.client = httpClient.CreateClient();
        }

        public async Task<T> CreateOrUpdateBooking<T>(BookingDetailsViewModel model, string accessToken)
        {
            throw new NotImplementedException();
        }

        public Task<T> DeleteBooking<T>(string id)
        {
            throw new NotImplementedException();
        }

        public Task<T> GetBookingById<T>(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<T> GetListBookings<T>(string filterCriteria = null)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = ApiType.GET,
                Url = $"getbookings/{filterCriteria}",
            });
        }
    }
}
