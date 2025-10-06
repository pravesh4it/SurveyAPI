namespace ABC.Models.DTO
{
    public class InvoiceSummaryDto
    {
        public Guid InvoiceId { get; set; }
        public string InvoiceNumber { get; set; } = string.Empty;
        public DateTime InvoiceDate { get; set; }
        public decimal GrandTotal { get; set; }
        public string? PaymentStatus { get; set; }
        public string? CreatedBy { get; set; }
    }
}
