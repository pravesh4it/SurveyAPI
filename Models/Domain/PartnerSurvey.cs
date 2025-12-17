using System.ComponentModel.DataAnnotations;

namespace ABC.Models.Domain
{
    public class PartnerSurvey
    {
        [Key]
        public Guid Id { get; set; } // Unique identifier
        public Guid PartnerId { get; set; } // Identifier for the partner
        public int AutoNumber { get; set; }   // New auto-increment-like field
        public decimal Rate { get; set; } // Rate, assuming it's a monetary value
        public int Quota { get; set; }
        public string? AvailableVariable { get; set; } // Available variable, adjust type as needed
        public string? PartnerSuccessLink { get; set; } // URL for partner success
        public string? PartnerDisqualificationLink { get; set; } // URL for disqualification
        public string? PartnerQuotaLink { get; set; } // URL for quota
        public string? PausedLink { get; set; } // URL for paused state
        public string? SecurityFailLink { get; set; } // URL for security fail
        public DateTime AddedOn { get; set; } // Timestamp for when added
        public string AddedBy { get; set; } // Identifier for the user who added it
        public Guid SurveyUuid { get; set; } // Unique identifier for the survey

        // Constructor
        public PartnerSurvey()
        {
            Id = Guid.NewGuid();
            AddedOn = DateTime.UtcNow;
        }
    }
}
