using Clinic.DTO.Models;
using Clinic.DTO.Models.Dto;

namespace Clinic.Invoice.Contracts
{
    public interface IInvoiceService
    {
        Task<string> GetInvoiceByIdAsync(string invoiceId);

        Task<InvoiceDetailsViewModel> AddInvoiceAsync(InvoiceDetailsViewModel invoice);
        Task<string> ExportInvoiceById(string bookingId);
    }
}
