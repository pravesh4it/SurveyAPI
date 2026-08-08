namespace ABC.Models.DTO
{
    public class SurveyPreScreeningResult
    {
        public bool HasPreScreening { get; set; }
        public bool HasInstruction { get; set; }
        public string InstructionText { get; set; } = "";
    }
}
