namespace ABC.Models.DTO
{
    public class InvoiceCreateDto
    {
        public Guid? SurveyId { get; set; }
        public string? ClientSurveyName { get; set; }
        public string? PONumber { get; set; }
        public string AccountEmail { get; set; } = null!;
        public string? AddrLine1 { get; set; }
        public string? AddrLine2 { get; set; }
        public string? AddrLine3 { get; set; }
        public string? ZipCode { get; set; }
        public DateTime? DueDate { get; set; }
        public string? CurrencyCode { get; set; } = "INR";
        public string? Notes { get; set; }
        public List<InvoiceItemDto> Items { get; set; } = new();
    }
}
