namespace ABC.Models.DTO
{
    public class SurveyEditDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public string Language { get; set; }
        public int? Completes { get; set; }
        public int LengthOfSurvey { get; set; }
        public decimal? Incidence { get; set; }
        public int FilledTime { get; set; }
        public string Status { get; set; } // Enum for Status
        public string Client { get; set; }
        public List<string> ProjectManagers { get; set; }
        public List<string> SalesManagers { get; set; }
        public string ClientLink { get; set; }
        public string CreatedById { get; set; }
        public DateOnly LaunchedDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public string Currency { get; set; }
        public decimal ClientIR { get; set; }
        public decimal ClientRate { get; set; }
        public int SurveyQuota { get; set; }
        public bool PreScreener { get; set; }

    }
}
