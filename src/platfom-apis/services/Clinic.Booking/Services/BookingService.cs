using System.Globalization;
using System.Text;
using System.Text.Json;
using AutoMapper;
using Clinic.Booking.Contracts;
using Clinic.Caching.Interfaces;
using Clinic.Common.Models;
using Clinic.Common.Options;
using Clinic.Common.Validator;
using Clinic.Data.Models;
using Clinic.DTO.Models.Dto;
using Clinic.DTO.Models.Message;
using Clinic.DTO.Models.Model;
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
        public async Task<ResponseDto> AddBookingAsync(BookingDetailsViewModel booking)
        {
            ResponseDto result = new();//create new object to store the final result of appointment adding process 
            ArgumentValidation.ThrowIfNull(booking);//ensure the booking argument is not null, if null -> throw an exception
           var getExistingBooking = await this.GetBookingAsync($" e.UserId = '{booking.UserId}' and e.OrderStatus= '{OrderStatus.Cart}' and e.id= '{booking.Id}'").ConfigureAwait(false);
                //call GetBookingAsync to get a list of appointments include UserID, OrderStatus and AppointmentID
           BookingDetailsViewModel? existingBooking = getExistingBooking.FirstOrDefault();
                //if appointment exists according to the given condition, it will take the first appointment from the list and assign it to the existingBooking variable.
            if (existingBooking != null){ //if appointment not null, method will update booking ID and Etag from existingBooking
            booking.Id = existingBooking.Id;
            booking.Etag = existingBooking.Etag;
            if(booking.OrderStatus == OrderStatus.Submitted.ToString()){
                
            }
            
            await this.UpdateBookingAsync(booking).ConfigureAwait(false);//calls the UpdateBookingAsync method to update the appointment and then returns the results.
                result.Result = booking;
            return result;
           }else{
            booking.OrderStatus = OrderStatus.Cart.ToString();
            booking.OrderTotal = booking.Products.Sum(x => x.Price);
            var bookingModel = autoMapper.Map<Data.Models.Booking>(booking);//maps the booking to a Booking object in database form and creates a StringContent from this object.
                using var bookingRequest = new StringContent(JsonSerializer.Serialize(bookingModel),Encoding.UTF8,ContentType);
            var bookingResponse = await this.httpClient.PostAsync(new Uri($"{applicationSettings.Value.DataStoreEndpoint}getbooking"),bookingRequest).ConfigureAwait(false);
                //request to the backend API to create a new appointment
           if (!bookingResponse.IsSuccessStatusCode){
                await this.ThrowServiceToServiceErrors(bookingResponse).ConfigureAwait(false);
            }
            var createdBookingDAO = await bookingResponse.Content.ReadFromJsonAsync<Clinic.Data.Models.Booking>().ConfigureAwait(false);
                //reads the generated appointment information from the response.
            var createdBooking = autoMapper.Map<BookingDetailsViewModel>(createdBookingDAO);//new appointment result is mapped to a BookingDetailsViewModel object and the result is returned
                result.Result = booking;
                return result;
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

        public async Task<ResponseDto> BookingSucess(string id)
        {
            ResponseDto result = new();
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
                result.Result = true;
                return result;
            }
            return result;
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

        public async Task<ResponseDto> GetBookingByIdAsync(string orderId)
        {
            ResponseDto result = new();
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
            result.Result = booking;
            return result;
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

        public async Task<HttpResponseMessage> UpdateSchedule(UpdateSchedule schedule)
        {
            using var userRequest = new StringContent(JsonSerializer.Serialize(schedule), Encoding.UTF8, ContentType);
            var userResponse = await httpClient.PutAsync(new Uri($"{applicationSettings.Value.IdentityApiEndpoint}/UpdateSchedule"), userRequest).ConfigureAwait(false);
            if (!userResponse.IsSuccessStatusCode)
            {
                await ThrowServiceToServiceErrors(userResponse).ConfigureAwait(false);
            }
            return userResponse;
        }
    }
}
