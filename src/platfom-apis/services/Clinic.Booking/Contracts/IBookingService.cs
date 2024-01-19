namespace Clinic.Booking.Contracts
{
    public interface IBookingService
    {
        Task<IEnumerable<BookingDetailsViewModel>> GetOrdersAsync(string? filterCriteria = null);
        Task<BookingDetailsViewModel> GetOrderByIdAsync(string orderId);
        Task<BookingDetailsViewModel> AddOrderAsync(BookingDetailsViewModel order);
        Task<HttpResponseMessage> UpdateOrderAsync(BookingDetailsViewModel order);
        double ComputeTotalDiscount(double orderTotal);
    }
}
