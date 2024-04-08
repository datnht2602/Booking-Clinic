using Clinic.DTO.Models;

namespace Clinic.Email.Contract;

public interface IEmailService
{
    Task SendInvoice(string email);
    Task ChangeSchedule(BookingDetailsViewModel model);
}