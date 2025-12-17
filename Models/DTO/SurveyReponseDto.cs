using ABC.Models.Domain;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ABC.Models.DTO
{
    public class SurveyResponseDto
    {
        
        public Guid? SurveyPartnerId { get; set; } // Foreign key to Survey table
        [Required]
        public string RespondentId { get; set; } // Unique identifier for the respondent
        public string? RespondentIP { get; set; }
        public string addedby { get; set; }
        public int AutoNumber { get; set; }
    }
}
