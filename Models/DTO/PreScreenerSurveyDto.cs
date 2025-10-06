namespace ABC.Models.DTO
{
    public class PreScreenerSurveyDto
    {
        public string SurveyId { get; set; }
        public string SurveyName { get; set; }
        public string SurveyTitle { get; set; }
        public List<SurveyPreScreenerDto> surveyPreScreenerDtos { get; set; }
    }
}
