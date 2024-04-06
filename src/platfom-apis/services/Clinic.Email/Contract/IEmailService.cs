namespace Clinic.Email.Contract;

public interface IEmailService
{
    Task SendInvoice(string email);
}