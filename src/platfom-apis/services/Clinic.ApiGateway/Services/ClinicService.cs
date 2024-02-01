using Clinic.ApiGateway.Contracts;
using Clinic.DTO.Models;

namespace Clinic.ApiGateway.Services
{
    public class ClinicService : IClinicService
    {
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

        public Task<IEnumerable<DoctorListViewModel>> GetDoctorsAsync(string filterCriteria = null)
        {
            throw new NotImplementedException();
        }

        public Task<InvoiceDetailsViewModel> GetInvoiceByIdAsync(string orderId)
        {
            throw new NotImplementedException();
        }

        public Task<InvoiceDetailsViewModel> SubmitOrder(BookingDetailsViewModel order)
        {
            throw new NotImplementedException();
        }
    }
}
