using System.ComponentModel.DataAnnotations;

namespace ABC.Models.DTO
{
    public class InvoiceItemRequestDto
    {
        [Required]
        public decimal Quantity { get; set; }

        [Required]
        public string Description { get; set; } = null!;

        // Unit price
        public decimal Unit { get; set; }

        // Optional: allow client to send computed total (server will recalc authoritatively)
        public decimal? Total { get; set; }

        public decimal? TaxAmount { get; set; }
        public decimal? DiscountAmount { get; set; }
    }
}
