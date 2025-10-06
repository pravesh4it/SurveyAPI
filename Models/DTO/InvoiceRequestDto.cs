using System.ComponentModel.DataAnnotations;

namespace ABC.Models.DTO
{
    public class InvoiceRequestDto
    {
        [Required]
        public string SurveyId { get; set; } = null!; // we accept string to allow GUID or custom id

        public string? ClientSurveyName { get; set; }
        public string? PONumber { get; set; }

        [Required, EmailAddress]
        public string AccountEmail { get; set; } = null!;

        public InvoiceAddressDto? Address { get; set; }

        public string? InvoiceDate { get; set; } // optional - server can ignore and use server time
        public List<InvoiceItemRequestDto> Rows { get; set; } = new List<InvoiceItemRequestDto>();

        public decimal? GrandTotal { get; set; }
    }
}
