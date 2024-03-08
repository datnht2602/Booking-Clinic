using System.Text;
using AutoMapper;
using Clinic.ApiGateway.Contracts;
using Clinic.Caching.Interfaces;
using Clinic.Common.Models;
using Clinic.Common.Options;
using Clinic.Common.Validator;
using Clinic.DTO.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Clinic.ApiGateway.Services
{
    public class ClinicService : IClinicService
    {
        private const string ContentType = "application/json";
        private readonly IOptions<ApplicationSettings> applicationSettings;
        private readonly HttpClient httpClient;
        private readonly IMapper autoMapper;
        private readonly IDistributedCacheService cacheService;
        public ClinicService(IHttpClientFactory httpClientFactory, IOptions<ApplicationSettings> applicationSettings, IMapper autoMapper, IDistributedCacheService cacheService)
        {
            ArgumentValidation.ThrowIfNull(applicationSettings);
             httpClient = httpClientFactory.CreateClient();
            this.autoMapper = autoMapper;
            this.cacheService = cacheService;
            this.applicationSettings = applicationSettings;
        }
        public async Task<BookingDetailsViewModel> CreateOrUpdateBooking(BookingDetailsViewModel model)
        {
            using var bookingRequest = new HttpRequestMessage(HttpMethod.Post, $"{applicationSettings.Value.ProductsApiEndpoint}/getbooking");
            bookingRequest.Content = new StringContent(JsonConvert.SerializeObject(model),
                Encoding.UTF8,"application/json");
            var bookingResponse = await httpClient.SendAsync(bookingRequest).ConfigureAwait(false);

            if(!bookingResponse.IsSuccessStatusCode)
            {
                await ThrowServiceToServiceErrors(bookingResponse).ConfigureAwait(false);
            }
            if(bookingResponse.StatusCode != System.Net.HttpStatusCode.NoContent)
            {
                var result = await bookingResponse.Content.ReadFromJsonAsync<BookingDetailsViewModel>().ConfigureAwait(false);
                

                return result;
            }
            else
            {
                return new BookingDetailsViewModel();
            }
        }

        public Task<BookingDetailsViewModel> GetBookingByIdAsync(string orderId)
        {
            throw new NotImplementedException();
        }

        public Task<DoctorDetailsViewModel> GetDoctorByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<DoctorListViewModel>> GetDoctorsAsync(string filterCriteria = null)
        {
            using var doctorRequest = new HttpRequestMessage(HttpMethod.Get, $"{applicationSettings.Value.IdentityApiEndpoint}/user/getlistdoctor");
            var doctorResponse = await httpClient.SendAsync(doctorRequest).ConfigureAwait(false);

            if(!doctorResponse.IsSuccessStatusCode)
            {
                await ThrowServiceToServiceErrors(doctorResponse).ConfigureAwait(false);
            }
            if(doctorResponse.StatusCode != System.Net.HttpStatusCode.NoContent)
            {
                var doctors = await doctorResponse.Content.ReadFromJsonAsync<List<ApplicationUserModel>>().ConfigureAwait(false);

                var doctorList = autoMapper.Map<List<DoctorListViewModel>>(doctors);
                return doctorList;
            }
            else
            {
                return new List<DoctorListViewModel>();
            }
        }

        public Task<InvoiceDetailsViewModel> GetInvoiceByIdAsync(string orderId)
        {
            throw new NotImplementedException();
        }

        public Task<InvoiceDetailsViewModel> SubmitOrder(BookingDetailsViewModel order)
        {
            throw new NotImplementedException();
        }

        public async Task<BookingViewModel> GetBookingDetail(string userId)
        {
            using var doctorRequest = new HttpRequestMessage(HttpMethod.Get, $"{applicationSettings.Value.IdentityApiEndpoint}/user/GetDoctorSchedule?userId={userId}");
            var doctorResponse = await httpClient.SendAsync(doctorRequest).ConfigureAwait(false);

            if(!doctorResponse.IsSuccessStatusCode)
            {
                await ThrowServiceToServiceErrors(doctorResponse).ConfigureAwait(false);
            }
            if(doctorResponse.StatusCode != System.Net.HttpStatusCode.NoContent)
            {
                var bookingResponse = await doctorResponse.Content.ReadFromJsonAsync<BookingViewModel>().ConfigureAwait(false);
                

                return bookingResponse;
            }
            else
            {
                return new BookingViewModel();
            }
            
        }

        private async Task ThrowServiceToServiceErrors(HttpResponseMessage response)
        {
            var exceptionReponse = await response.Content.ReadFromJsonAsync<ExceptionResponse>().ConfigureAwait(false);
            throw new Exception(exceptionReponse.InnerException);
        }
    }
}
