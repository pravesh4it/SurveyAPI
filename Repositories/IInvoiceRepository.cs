using ABC.Models.Domain;
using ABC.Models.DTO;

namespace ABC.Repositories
{
    public interface IInvoiceRepository
    {
        Task<(Guid invoiceId, string invoiceNumber)> CreateInvoiceAsync(InvoiceCreateDto dto, string? createdBy = null);
        Task<InvoiceMaster?> GetInvoiceByIdAsync(Guid invoiceId);
        Task<List<InvoiceMasterDto>> GetInvoicesBySurveyIdAsync(Guid surveyId);
        Task<InvoiceMasterDto?> GetInvoiceDtoByIdAsync(Guid invoiceId);
    }
}