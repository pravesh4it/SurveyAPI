using System.ComponentModel.DataAnnotations;

namespace ABC.Models.Domain
{
    public class Country
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(10)]
        public string Currency { get; set; }

        [Required]
        [MaxLength(5)]
        public string CurrencySymbol { get; set; }

        [Required]
        [MaxLength(10)]
        public string IsdCode { get; set; }

        [Required]
        [MaxLength(10)]
        public string ShortCode { get; set; }
    }
}
