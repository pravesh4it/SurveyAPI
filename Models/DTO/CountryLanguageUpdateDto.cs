using System.ComponentModel.DataAnnotations;

namespace ABC.Models.DTO
{
    public class CountryLanguageUpdateDto
    {
        [Required]
        public Guid CountryId { get; set; }

        /// <summary>
        /// The new set of language ids — existing mappings for the country will be replaced.
        /// </summary>
        [Required]
        public List<Guid> LanguageIds { get; set; } = new();

        /// <summary>
        /// Which language should be primary after update (optional).
        /// </summary>
        public Guid? PrimaryLanguageId { get; set; }
    }
}
