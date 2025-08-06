namespace ABC.Models.DTO
{
    public class RecontactDto
    {
            public string SurveyId { get; set; }
            public string Description { get; set; }
            public string CreatedById { get; set; }
            public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
            public IFormFile file { get; set; }
    }
}
