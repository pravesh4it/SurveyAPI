using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ABC.Models.Domain
{
    public class SurveyResponsesPreScreener
    {
        public Guid Id { get; set; }
        [Required]
        public int SurveyResponsesId { get; set; }
        public Guid PreScreenerId { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public string Answer { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }
        [Required]
        public string AddedBy { get; set; } = string.Empty;

    }
}
