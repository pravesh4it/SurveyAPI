namespace ABC.Models.DTO
{
    public class SurveySearchResponse
    {
        public int TotalRecords { get; set; }

        public List<SurveyDto> Data { get; set; } = new();
    }
}
