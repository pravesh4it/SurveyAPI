namespace ABC.Models.DTO
{
    public class InvoiceItemDto
    {
        public decimal Quantity { get; set; } = 1m;
        public string? Description { get; set; }
        public decimal UnitCost { get; set; } = 0m;
        public decimal TaxAmount { get; set; } = 0m;
        public decimal DiscountAmount { get; set; } = 0m;
        public int LineNo { get; set; }
        public decimal LineTotal { get; set; }
    }
}
