using System.Globalization;
using System.Text;
using System.Text.Json;
using AutoMapper;
using Clinic.Booking.Contracts;
using Clinic.Caching.Interfaces;
using Clinic.Common.Models;
using Clinic.Common.Options;
using Clinic.Common.Validator;
using Clinic.DTO.Models.Message;
using Clinic.Message;
using Microsoft.Extensions.Options;

namespace Clinic.Booking.Services
{
    public class BookingService : IBookingService
    {
        
        private const string ContentType = "application/json";
        private readonly IOptions<ApplicationSettings> applicationSettings;
        private readonly HttpClient httpClient;
        private readonly IMapper autoMapper;
        private readonly IDistributedCacheService cacheService;
        private readonly IMessageBus messageBus;
        public BookingService(IHttpClientFactory httpClientFactory, IOptions<ApplicationSettings> applicationSettings, IMapper autoMapper, IDistributedCacheService cacheService, IMessageBus messageBus)
        {
            ArgumentValidation.ThrowIfNull(applicationSettings);
             httpClient = httpClientFactory.CreateClient();
            this.autoMapper = autoMapper;
            this.cacheService = cacheService;
            this.applicationSettings = applicationSettings;
            this.messageBus = messageBus;
        }
        public async Task<BookingDetailsViewModel> AddBookingAsync(BookingDetailsViewModel booking)
        {
           ArgumentValidation.ThrowIfNull(booking);
           var getExistingBooking = await this.GetBookingAsync($" e.UserId = '{booking.UserId}' and e.OrderStatus= '{OrderStatus.Cart}' and e.id= '{booking.Id}'").ConfigureAwait(false);
           BookingDetailsViewModel? existingBooking = getExistingBooking.FirstOrDefault();
           if(existingBooking != null){
            booking.Id = existingBooking.Id;
            booking.Etag = existingBooking.Etag;
            if(booking.OrderStatus == OrderStatus.Submitted.ToString()){
                
            }
            
            await this.UpdateBookingAsync(booking).ConfigureAwait(false);
            return booking;
           }else{
            booking.OrderStatus = OrderStatus.Cart.ToString();
            booking.OrderTotal = booking.Products.Sum(x => x.Price);
            var bookingModel = autoMapper.Map<Data.Models.Booking>(booking);
            using var bookingRequest = new StringContent(JsonSerializer.Serialize(bookingModel),Encoding.UTF8,ContentType);
            var bookingResponse = await this.httpClient.PostAsync(new Uri($"{applicationSettings.Value.DataStoreEndpoint}getbooking"),bookingRequest).ConfigureAwait(false);

            if(!bookingResponse.IsSuccessStatusCode){
                await this.ThrowServiceToServiceErrors(bookingResponse).ConfigureAwait(false);
            }
            var createdBookingDAO = await bookingResponse.Content.ReadFromJsonAsync<Clinic.Data.Models.Booking>().ConfigureAwait(false);

            var createdBooking = autoMapper.Map<BookingDetailsViewModel>(createdBookingDAO);
            return createdBooking;
           }
        }

        private async Task ThrowServiceToServiceErrors(HttpResponseMessage response)
        {
            var exceptionReponse = await response.Content.ReadFromJsonAsync<ExceptionResponse>().ConfigureAwait(false);
            throw new Exception(exceptionReponse.InnerException);
        }

        public double ComputeTotalDiscount(double orderTotal)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> BookingSucess(string id)
        {
            var getExistingBooking = await this
                .GetBookingAsync(
                    $" e.id= '{id}'")
                .ConfigureAwait(false);
            BookingDetailsViewModel? existingBooking = getExistingBooking.FirstOrDefault();
            if (existingBooking != null)
            {
                existingBooking.OrderStatus = OrderStatus.Submitted.ToString();
                await this.UpdateBookingAsync(existingBooking).ConfigureAwait(false);
                var bookingDto = this.autoMapper.Map<BookingDetailDto>(existingBooking);
                await messageBus.PublishMessage(bookingDto, "checkoutmessagetopic");
                return true;
            }
            return false;
        }

        public async Task<IEnumerable<BookingDetailsViewModel>> GetBookingAsync(string? filterCriteria = null)
        {
            using var bookingRequest = new HttpRequestMessage(HttpMethod.Get, $"{applicationSettings.Value.DataStoreEndpoint}getallbooking?filterCriteria={filterCriteria}");
            var bookingResponse = await httpClient.SendAsync(bookingRequest).ConfigureAwait(false);

            if(!bookingResponse.IsSuccessStatusCode){
                await ThrowServiceToServiceErrors(bookingResponse).ConfigureAwait(false);
            }
            if(bookingResponse.StatusCode != System.Net.HttpStatusCode.NoContent){
                var bookings = await bookingResponse.Content.ReadFromJsonAsync<IEnumerable<Clinic.Data.Models.Booking>>().ConfigureAwait(false);

                var bookingList = autoMapper.Map<List<BookingDetailsViewModel>>(bookings);
                return bookingList;
            }else{
                return new List<BookingDetailsViewModel>();
            }
        }

        public async Task<BookingDetailsViewModel> GetBookingByIdAsync(string orderId)
        {
           BookingDetailsViewModel? booking = null;
           using var bookingRequest = new HttpRequestMessage(HttpMethod.Get,$"{applicationSettings.Value.DataStoreEndpoint}getbooking/{orderId}");
           var bookingResponse = await httpClient.SendAsync(bookingRequest).ConfigureAwait(false);
           if(!bookingResponse.IsSuccessStatusCode){
            await ThrowServiceToServiceErrors(bookingResponse).ConfigureAwait(false);
           }
           if(bookingResponse.StatusCode != System.Net.HttpStatusCode.NoContent){
            var bookingDAO = await bookingResponse.Content.ReadFromJsonAsync<Clinic.Data.Models.Booking>().ConfigureAwait(false);
            booking = autoMapper.Map<BookingDetailsViewModel>(bookingDAO);
           }
           return booking;
        }

        public async Task<HttpResponseMessage> UpdateBookingAsync(BookingDetailsViewModel bookingUpdate)
        {
            var booking = autoMapper.Map<Data.Models.Booking>(bookingUpdate);
            using var bookingRequest = new StringContent(JsonSerializer.Serialize(booking),Encoding.UTF8,ContentType);
            var bookingResponse = await httpClient.PutAsync(new Uri($"{applicationSettings.Value.DataStoreEndpoint}getbooking"),bookingRequest).ConfigureAwait(false);
            if(!bookingResponse.IsSuccessStatusCode){
                await ThrowServiceToServiceErrors(bookingResponse).ConfigureAwait(false);
            }
            return bookingResponse;
        }
    }
}
