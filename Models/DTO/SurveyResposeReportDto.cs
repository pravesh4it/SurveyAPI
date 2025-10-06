namespace ABC.Models.DTO
{
    public class SurveyResposeReportDto
    {
            public string Id { get; set; }
            public string SurveyId { get; set; }
            public string RespondentId { get; set; }
            public string RespondentIP { get; set; }
            public string Status { get; set; }
            public string PartnerName { get; set; }
            public string SurveyName { get; set; } 
            public DateTime? CreatedDate { get; set; }
            public DateTime? CompletedDate { get; set; }

            public string LOI { get; set; }
    }
}
