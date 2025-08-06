
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ABC.Models.Domain
{
    public class SurveyPreScreener
    {
        [Key]
        public Guid Id { get; set; }

        public string SurveyId { get; set; }

        [Required]
        public string QuestionType { get; set; } = string.Empty;

        [Required]
        public string Question { get; set; } = string.Empty;

        public string? Option1 { get; set; }
        public string? Option2 { get; set; }
        public string? Option3 { get; set; }
        public string? Option4 { get; set; }

        public bool IsDelete { get; set; } = false;

        public DateTime AddedOn { get; set; } = DateTime.UtcNow;

        [Required]
        public string AddedBy { get; set; } = string.Empty;
    }
}

