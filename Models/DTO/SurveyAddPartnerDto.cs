using System.ComponentModel.DataAnnotations;

namespace ABC.Models.DTO
{
    public class SurveyAddPartnerDto
    {
        [Required]
        public Guid PartnerId { get; set; } // Identifier for the partner

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Rate must be a non-negative value.")]
        public decimal Rate { get; set; } // Rate, assuming a monetary or percentage value

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Rate must be a non-negative value.")]
        public int Quota { get; set; } // Rate, assuming a monetary or percentage value

        [MaxLength(500)]
        public string AvailableVariable { get; set; } // Optional variable, max length for safety
        public string? PartnerSuccessLink { get; set; } // URL for partner success
        public string? PartnerDisqualificationLink { get; set; } // Optional URL for disqualification
        public string? PartnerQuotaLink { get; set; } // Optional URL for quota
        public string? PausedLink { get; set; } // Optional URL for paused state
        public string? SecurityFailLink { get; set; } // Optional URL for security fail

        [Required]
        public string AddedBy { get; set; } // Identifier for the user adding the partner survey

        [Required]
        public Guid SurveyUuid { get; set; } // Unique identifier for the survey
    }
}
