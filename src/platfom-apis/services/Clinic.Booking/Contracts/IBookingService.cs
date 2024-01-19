namespace Clinic.Booking.Contracts
{
    public interface IBookingService
    {
        Task<IEnumerable<BookingDetailsViewModel>> GetBookingAsync(string? filterCriteria = null);
        Task<BookingDetailsViewModel> GetBookingByIdAsync(string orderId);
        Task<BookingDetailsViewModel> AddBookingAsync(BookingDetailsViewModel booking);
        Task<HttpResponseMessage> UpdateBookingAsync(BookingDetailsViewModel booking);
        double ComputeTotalDiscount(double bookingTotal);
    }
}
