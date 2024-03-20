using Clinic.DTO.Models.Dto;

namespace Clinic.Booking.Contracts
{
    public interface IBookingService
    {
        Task<IEnumerable<BookingDetailsViewModel>> GetBookingAsync(string? filterCriteria = null);
        Task<ResponseDto> GetBookingByIdAsync(string orderId);
        Task<ResponseDto> AddBookingAsync(BookingDetailsViewModel booking);
        Task<HttpResponseMessage> UpdateBookingAsync(BookingDetailsViewModel booking);
        double ComputeTotalDiscount(double bookingTotal);
        Task<ResponseDto> BookingSucess(string id);
    }
}
