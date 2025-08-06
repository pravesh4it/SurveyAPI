using System.ComponentModel.DataAnnotations;

namespace ABC.Models.DTO
{
    public class PreScreenerAddDto
    {
            public string SurveyId { get; set; }

            [Required]
            public string QuestionType { get; set; } = string.Empty;
            [Required]
            public string Question { get; set; } = string.Empty;
            public string? Option1 { get; set; }

            [Required]
            public string AddedBy { get; set; } = string.Empty;
       
    }
}
