namespace ABC.Models.DTO
{
    public class GenerateVendorAllocationRequest
    {
        public Guid SurveyId { get; set; }

        public List<string> RespondentIds { get; set; }
        public bool GenerateAllocation { get; set; }
    }
}
