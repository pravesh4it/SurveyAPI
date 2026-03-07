namespace ABC.Models.DTO
{
    public class PreScreenerUpdateDto
    {
        public Guid Id { get; set; }
        public string? SurveyId { get; set; }
        public string QuestionType { get; set; }
        public string Question { get; set; }
        public string Option1 { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
