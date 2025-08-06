namespace ABC.Models.DTO
{
    public class SurveyQuestioinResponseDto
    {
        public string AddedBy { get; set; }
        public string RespondentIP { get; set; }
        public string RespondentId { get; set; }
        public string SurveyPartnerId { get; set; }
        public List<ResponseDto> Responses { get; set; }
    }

    public class ResponseDto
    {
        public string QuestionId { get; set; }
        public string Answer { get; set; }
    }

}
