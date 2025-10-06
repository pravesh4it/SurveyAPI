using System.ComponentModel.DataAnnotations;

namespace ABC.Models.DTO
{
    public class CountryLanguageCreateDto
    {
        [Required]
        public Guid CountryId { get; set; }

        /// <summary>
        /// Language ids to add (from MultiSelect.Id). Duplicates will be ignored.
        /// </summary>
        [Required]
        public List<Guid> LanguageIds { get; set; } = new();

        /// <summary>
        /// Optional: mark one of the languages as primary — pass the languageId if needed.
        /// </summary>
        public Guid? PrimaryLanguageId { get; set; }
    }
}
