namespace ABC.Models.Domain
{
    public class UserProfile
    {
        public Guid Id { get; set; }

        public string UserId { get; set; }

        // Personal Info
        public string FullName { get; set; }
        public DateTime Dob { get; set; }
        public string Gender { get; set; }
        public string Country { get; set; }
        public string Zip { get; set; }
        public string Income { get; set; }

        // Demographics
        public string MaritalStatus { get; set; }
        public string Children { get; set; }
        public string HouseholdSize { get; set; }
        public string EducationLevel { get; set; }
        public string EmploymentStatus { get; set; }

        // Technology
        public string Devices { get; set; } // Comma-separated
        public string ShoppingFrequency { get; set; }

        // Travel
        public string DomesticTravel { get; set; }
        public string InternationalTravel { get; set; }

        // Media
        public string MediaPlatforms { get; set; }

        // Health
        public string HealthStatus { get; set; }
        public string Chronic { get; set; }
        public string SmokingAlcohol { get; set; }
        public string Exercise { get; set; }
        public string Diet { get; set; }

        // Survey Preferences
        public string SurveyLength { get; set; }
        public string SurveyFormats { get; set; }
        public string SurveyFrequency { get; set; }

        // Incentives
        public string RewardType { get; set; }
        public string RewardFrequency { get; set; }

        // Additional
        public string ReferralSource { get; set; }

        // Consent
        public bool AcceptTerms { get; set; }
        public bool AcceptPrivacy { get; set; }
        public bool AcceptDataProcessing { get; set; }
        public bool SubscribeNewsletter { get; set; }
        public string ProfileURL { get; set; }

        public DateTime CreatedOn { get; set; }
        public string? PasswordResetCode { get; set; }
        public DateTime? ResetCodeSentDate { get; set; }
    }

}
