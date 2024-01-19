using Clinic.DTO.Models;

namespace Clinic.Invoice.Contracts
{
    public interface IInvoiceService
    {
        Task<InvoiceDetailsViewModel> GetInvoiceByIdAsync(string invoiceId);

        Task<InvoiceDetailsViewModel> AddInvoiceAsync(InvoiceDetailsViewModel invoice);
    }
}
