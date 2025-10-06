namespace ABC.Models.DTO
{
    public class DashboardOverviewDto
    {
        public int TotalSurveys { get; set; }
        public int ActiveSurveys { get; set; }
        public int ClosedSurveys { get; set; }
        public int DraftSurveys { get; set; }
        public int TotalResponses { get; set; }
        public double ResponseRate { get; set; } // 0-100
        public List<StatusCountDto> StatusDistribution { get; set; } = new();
    }
    
    public class StatusCountDto
    {
        public string Status { get; set; } = default!;
        public int Count { get; set; }
    }

    public class TimePointDto
    {
        public DateTime Date { get; set; } // UTC date (00:00)
        public int Count { get; set; }
    }

    public class TopSurveyDto
    {
        public Guid SurveyId { get; set; }
        public string Name { get; set; } = default!;
        public int Responses { get; set; }
    }

    public class RecentSurveyRowDto
    {
        public Guid SurveyId { get; set; }
        public string Name { get; set; } = default!;
        public string Status { get; set; } = default!;
        public DateTime CreatedAt { get; set; }
        public DateOnly? EndAt { get; set; }
        public int Responses { get; set; }
        public string Owner { get; set; } = default!;
    }

    public class UpcomingSurveyDto
    {
        public Guid SurveyId { get; set; }
        public string Name { get; set; } = default!;
        public DateTime StartAt { get; set; }
        public string Owner { get; set; } = default!;
    }

    public class AlertDto
    {
        public string Type { get; set; } = "info"; // info|warning|error
        public string Text { get; set; } = default!;
    }
}
