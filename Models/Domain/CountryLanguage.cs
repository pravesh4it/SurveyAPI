using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ABC.Models.Domain
{
    public class CountryLanguage
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid CountryId { get; set; }

        [Required]
        public Guid MultiSelectId { get; set; }  // refers to MultiSelect.Id

        // Navigation properties
        [ForeignKey(nameof(CountryId))]
        public Country Country { get; set; }

        [ForeignKey(nameof(MultiSelectId))]
        public MultiSelect Language { get; set; }

        // Optionally store extra metadata, e.g. isPrimary, addedOn etc.
        public bool IsPrimary { get; set; } = false;
    }
}
