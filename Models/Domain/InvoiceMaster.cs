namespace ABC.Models.Domain
{
    public class InvoiceMaster
    {
        public Guid InvoiceId { get; set; } = Guid.NewGuid();
        public string InvoiceNumber { get; set; } = null!; // generated in application or DB
        public Guid? SurveyId { get; set; }
        public string? ClientSurveyName { get; set; }
        public string? PONumber { get; set; }
        public string AccountEmail { get; set; } = null!;
        public string? AddrLine1 { get; set; }
        public string? AddrLine2 { get; set; }
        public string? AddrLine3 { get; set; }
        public string? ZipCode { get; set; }
        public DateTime InvoiceDate { get; set; } = DateTime.UtcNow;
        public DateTime? DueDate { get; set; }
        public bool PaymentDone { get; set; } = false;
        public string? PaymentStatus { get; set; }
        public DateTime? PaymentTermsDueDate { get; set; }
        public decimal GrandTotal { get; set; } = 0m;
        public decimal TaxTotal { get; set; } = 0m;
        public decimal DiscountTotal { get; set; } = 0m;
        public decimal AdditionalAmount { get; set; } = 0m;
        public string? CurrencyCode { get; set; } = "INR";
        public string? Notes { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // navigation
        public ICollection<InvoiceTransaction> Transactions { get; set; } = new List<InvoiceTransaction>();
    }
}
