using System.ComponentModel.DataAnnotations;

namespace ABC.Models.DTO
{
    public class SurveyCompleteResponseDto
    {
        [Required]
        public string uid { get; set; } // Foreign key to Survey table
        public string? sid { get; set; } // Foreign key to Survey table
        [Required]
        public string response_type { get; set; } // Unique identifier for the respondent
        public string addedby { get; set; }
    }
}
