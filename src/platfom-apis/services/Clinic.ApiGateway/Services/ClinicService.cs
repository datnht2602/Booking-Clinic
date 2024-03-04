using AutoMapper;
using Clinic.ApiGateway.Contracts;
using Clinic.Caching.Interfaces;
using Clinic.Common.Models;
using Clinic.Common.Options;
using Clinic.Common.Validator;
using Clinic.DTO.Models;
using Microsoft.Extensions.Options;

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
        public Task<BookingDetailsViewModel> CreateOrUpdateBooking(BookingDetailsViewModel model)
        {
            throw new NotImplementedException();
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

        public Task<List<long>> GetListTime(string userId)
        {
            using var doctorRequest = new HttpRequestMessage(HttpMethod.Get, $"{applicationSettings.Value.IdentityApiEndpoint}/user/getlistdoctor");
            
        }

        private async Task ThrowServiceToServiceErrors(HttpResponseMessage response)
        {
            var exceptionReponse = await response.Content.ReadFromJsonAsync<ExceptionResponse>().ConfigureAwait(false);
            throw new Exception(exceptionReponse.InnerException);
        }
    }
}
