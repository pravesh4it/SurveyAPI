namespace ABC.Models.DTO
{
    public class SurveyViewDto
    {
        public string SurveyId { get; set; }
        public string SurveyTitle { get; set; }
        public string SurveyName { get; set; }
        public string LanguageId { get; set; }
        public string Language { get; set; }
        public string CountryId { get; set; }
        public string Country { get; set; }
        public int FilledTimeInDays { get; set; }
        public int Completes { get; set; }
        public int LengthOfSurveyInMinutes { get; set; }
        public decimal Incidence { get; set; }
        public string StatusId { get; set; }
        public string Status { get; set; }
        public string ClientName { get; set; }
        public string ClientLink { get; set; }
        public string ClientLaunchedDate { get; set; }
        public string ClientEndDate { get; set; }
        public string CreatedById { get; set; }
        public int ClientQuota { get; set; }
        public string ClientIR { get; set; }
        public string ClientRate { get; set; }
        public string SuccessLink { get; set; }
        public string DisqualificationLink { get; set; }
        public string QuotaFullLink { get; set; }
        public string CurrencySymbol { get; set; }
        public string CurrencyId { get; set; }
        public string CurrencyName { get; set; }
        public string MarginPercentage { get; set; }
        public string TotalProfit { get; set; }
        public string DropsPercentage { get; set; }
        public bool PreScreener { get; set; }
        public bool UniqueLink { get; set; }
        public string Link { get; set; }
        public string LOI { get; set; }
        public string IR { get; set; }
        public string LastCompleted { get; set; } // Nullable to allow for no completion date


    }
}
