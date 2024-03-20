using System.Text;
using AutoMapper;
using Clinic.ApiGateway.Contracts;
using Clinic.Caching.Interfaces;
using Clinic.Common.Models;
using Clinic.Common.Options;
using Clinic.Common.Validator;
using Clinic.DTO.Models;
using Clinic.DTO.Models.Dto;
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
            using var bookingRequest = new HttpRequestMessage(HttpMethod.Post, $"{applicationSettings.Value.OrdersApiEndpoint}/getbooking");
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

        public async Task<BookingDetailsViewModel> GetBookingByIdAsync(string orderId)
        {
            BookingDetailsViewModel order = new ();
            using var orderRequest = new HttpRequestMessage(HttpMethod.Get, $"{this.applicationSettings.Value.OrdersApiEndpoint}/getbooking/{orderId}");
            var productResponse = await this.httpClient.SendAsync(orderRequest).ConfigureAwait(false);
            if (!productResponse.IsSuccessStatusCode)
            {
                await this.ThrowServiceToServiceErrors(productResponse).ConfigureAwait(false);
            }

            if (productResponse.StatusCode != System.Net.HttpStatusCode.NoContent)
            {
                order = await productResponse.Content.ReadFromJsonAsync<BookingDetailsViewModel>().ConfigureAwait(false);
            }

            return order;
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

        public async Task<InvoiceDetailsViewModel> GetInvoiceByIdAsync(string orderId)
        {
            using var invoiceRequest = new HttpRequestMessage(HttpMethod.Get, $"{applicationSettings.Value.InvoiceApiEndpoint}/getinvoice/{orderId}");
            var invoiceResponse = await httpClient.SendAsync(invoiceRequest).ConfigureAwait(false);

            if (!invoiceResponse.IsSuccessStatusCode)
            {
                await ThrowServiceToServiceErrors(invoiceResponse).ConfigureAwait(false);
            }
            if (invoiceResponse.StatusCode != System.Net.HttpStatusCode.NoContent)
            {
                var invoiceModelResponse = await invoiceResponse.Content.ReadFromJsonAsync<InvoiceDetailsViewModel>().ConfigureAwait(false);


                return invoiceModelResponse;
            }
            else
            {
                return new InvoiceDetailsViewModel();
            }
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

        public async Task<object> GetDiscountForCode(string code)
        {
            using var couponRequest = new HttpRequestMessage(HttpMethod.Get, $"{applicationSettings.Value.CouponApiEndpoint}/getcoupon/{code}");
            var couponResponse = await httpClient.SendAsync(couponRequest).ConfigureAwait(false);

            if(!couponResponse.IsSuccessStatusCode)
            {
                await ThrowServiceToServiceErrors(couponResponse).ConfigureAwait(false);
            }
            if(couponResponse.StatusCode != System.Net.HttpStatusCode.NoContent)
            {
                var result = await couponResponse.Content.ReadFromJsonAsync<CouponDto>().ConfigureAwait(false);
                

                return result;
            }
            else
            {
                return new CouponDto();
            }
        }

        public async Task<bool> BookingSuccess(string id)
        {
            using var doctorRequest = new HttpRequestMessage(HttpMethod.Get, $"{applicationSettings.Value.OrdersApiEndpoint}/bookingsucess/{id}");
            var doctorResponse = await httpClient.SendAsync(doctorRequest).ConfigureAwait(false);

            if(!doctorResponse.IsSuccessStatusCode)
            {
                await ThrowServiceToServiceErrors(doctorResponse).ConfigureAwait(false);
            }
            if(doctorResponse.StatusCode != System.Net.HttpStatusCode.NoContent)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private async Task ThrowServiceToServiceErrors(HttpResponseMessage response)
        {
            var exceptionReponse = await response.Content.ReadFromJsonAsync<ExceptionResponse>().ConfigureAwait(false);
            throw new Exception(exceptionReponse.InnerException);
        }
    }
}
