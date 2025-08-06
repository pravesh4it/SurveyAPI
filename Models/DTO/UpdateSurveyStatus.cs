namespace ABC.Models.DTO
{
    public class UpdateSurveyStatusDto
    {
        public string Id { get; set; } = string.Empty;
        public string SurveyId { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string StatusId { get; set; } = string.Empty;
    }
}
