namespace ABC.Models.DTO
{
    public class VendorAllocationDto
    {
        public string Id { get; set; }
        public Guid VendorId { get; set; }
        public string SurveyId { get; set; }
        public string VendorName { get; set; }
        public string PONumber { get; set; }
        public int IdsCount { get; set; }
        public DateTime Created { get; set; }
        public List<RespondentAllocationDto> RespondentIds { get; set; }
        public List<VendorMailHistoryDto> MailHistory { get; set; }

    }
}
