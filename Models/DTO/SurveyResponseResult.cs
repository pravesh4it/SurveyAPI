namespace ABC.Models.DTO
{
    public class SurveyResponseResult
    {
        public string Status { get; set; }
        public string Message { get; set; }

        public string? RedirectLink { get; set; }
        public string? SurveyId { get; set; }
        public string SurveyName { get; set; }
        public string IpAddress { get; set; }
        public string UserId { get; set; }
    }
}
