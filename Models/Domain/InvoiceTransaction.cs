namespace ABC.Models.Domain
{
    public class InvoiceTransaction
    {
        public Guid InvoiceTxId { get; set; } = Guid.NewGuid();
        public Guid InvoiceId { get; set; }

        // line number (1,2,3...)
        public int LineNo { get; set; }

        public decimal Quantity { get; set; } = 1m;
        public string? Description { get; set; }
        public decimal UnitCost { get; set; } = 0m;
        public decimal LineTotal { get; set; } = 0m;
        public decimal TaxAmount { get; set; } = 0m;
        public decimal DiscountAmount { get; set; } = 0m;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? CreatedBy { get; set; }

        // navigation
        public InvoiceMaster? InvoiceMaster { get; set; }
    }
}
