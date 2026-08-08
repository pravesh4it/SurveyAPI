namespace ABC.Models.DTO
{
    public class PartnerSurveyDto
    {
        public Guid Id { get; set; }
        public Guid PartnerId { get; set; }
        public string PartnerName { get; set; }
        public decimal Rate { get; set; }
        public int Quota { get; set; }
        public string AvailableVariable { get; set; }
        public string SuccessLink { get; set; }
        public string DisqualificationLink { get; set; }
        public string QuotaLink { get; set; }
        public string PausedLink { get; set; }
        public string SecurityFailLink { get; set; }
        public DateTime AddedOn { get; set; }
        public string AddedBy { get; set; }
        public string SurveyUuid { get; set; }
        public string Link { get; set; }
        public string IR { get; set; }
        public string Drops { get; set; }
        public bool PreScreener { get; set; }
        public string LOI { get; set; }

        public string LastCompleted { get; set; }

        public string Statics { get; set; }

        public bool? ShowInstruction { get; set; }
        public string? InstructionText { get; set; }

        public int? Clicks { get; set; }

    }
}
