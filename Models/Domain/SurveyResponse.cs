using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ABC.Models.Domain
{
    public class SurveyResponse
    {
        public string Id { get; set; } // Unique identifier for each response
        [Required]
        public Guid SurveyId { get; set; } // Foreign key to Survey table
        [Required]
        public string RespondentId { get; set; } // Unique identifier for the respondent
        [Required]
        public DateTime ResponseDate { get; set; } = DateTime.UtcNow; // Date when the response was submitted
        [Required]
        [MaxLength(50)]
        public string Status { get; set; } // Status of the response (e.g., Completed, Disqualified, Quota Full)
        public int? CompletionTimeInMinutes { get; set; } // Time taken to complete the survey in minutes
        public string Answers { get; set; } // JSON or serialized format of the survey answers
        public string? RespondentIP { get; set; }
        public string? PartnerId { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Record creation timestamp
        public DateTime? UpdatedAt { get; set; } // Record last update timestamp
        public string AddedBy { get; set; } // Identifier for the user who added it
        // Navigation property to Survey (optional)
        [ForeignKey(nameof(SurveyId))]
        public virtual Survey Survey { get; set; }
        public string ClientURL { get; set; }
        public string? passcode { get; set; }
        public string? userIdFor { get; set; }

        // 🔹 Link to SurveyFile (for file tracking)
        public Guid? SurveyFileId { get; set; }
        [ForeignKey(nameof(SurveyFileId))]
        public virtual SurveyFile SurveyFile { get; set; }
        public bool IsRecontact { get; set; } = false;

    }

}
