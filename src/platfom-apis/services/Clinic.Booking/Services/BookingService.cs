using Clinic.Booking.Contracts;

namespace Clinic.Booking.Services
{
    public class BookingService : IBookingService
    {
        public Task<BookingDetailsViewModel> AddOrderAsync(BookingDetailsViewModel order)
        {
            throw new NotImplementedException();
        }

        public double ComputeTotalDiscount(double orderTotal)
        {
            throw new NotImplementedException();
        }

        public Task<BookingDetailsViewModel> GetOrderByIdAsync(string orderId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<BookingDetailsViewModel>> GetOrdersAsync(string? filterCriteria = null)
        {
            throw new NotImplementedException();
        }

        public Task<HttpResponseMessage> UpdateOrderAsync(BookingDetailsViewModel order)
        {
            throw new NotImplementedException();
        }
    }
}
