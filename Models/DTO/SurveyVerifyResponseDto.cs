using System.ComponentModel.DataAnnotations;

namespace ABC.Models.DTO
{
    public class SurveyVerifyResponseDto
    {

        [Required]
        public string RespondentId { get; set; } // Unique identifier for the respondent

        [Required]
        public string surveyId { get; set; } // Foreign key to Survey table
        public string? RespondentIP { get; set; }
        public string PassCode { get; set; }

    }
}
