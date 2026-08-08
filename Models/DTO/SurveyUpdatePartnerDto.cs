namespace ABC.Models.DTO
{
    public class SurveyUpdatePartnerDto
    {
        public Guid PartnerSurveyId { get; set; }
        public Guid PartnerId { get; set; }
        public string? AvailableVariable { get; set; }
        public int Quota { get; set; }
        public decimal Rate { get; set; }
        public Guid UpdatedBy { get; set; }

        public string PartnerQuotaLink { get; set; }
        public string PartnerSuccessLink { get; set; }
        public string PartnerDisqualificationLink { get; set; }
        public string SecurityFailLink { get; set; }
        public string PausedLink { get; set; }
        public bool PreScreenerAllowed { get; set; }
        // NEW FIELDS
        public bool ShowInstruction { get; set; }
        public string InstructionText { get; set; }
    }

}
