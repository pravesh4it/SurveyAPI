using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ABC.Models.Domain
{
    public class MultiSelect
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string DisplayName { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public int? ParentId { get; set; }

        public string Description { get; set; }

        [Required]
        [DefaultValue("")]
        public string SelectionType { get; set; }
        // Navigation
        public ICollection<CountryLanguage> CountryLanguages { get; set; } = new List<CountryLanguage>();
    }
}
