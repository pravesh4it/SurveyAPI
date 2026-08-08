namespace ABC.Models.DTO
{
    public class SurveySearchRequest
    {
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 20;

        public string? SurveyName { get; set; }

        public string? Title { get; set; }

        public string? ClientId { get; set; }

        public string? CountryId { get; set; }

        public string? StatusId { get; set; }

        public string? ProjectManagerId { get; set; }

        public string? SalesManagerId { get; set; }

        public bool MySurveysOnly { get; set; }

        public string? UserId { get; set; }
    }
}
