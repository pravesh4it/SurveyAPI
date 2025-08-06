namespace ABC.Models.DTO
{
    public class RegisterUserDto
    {
        public ConsentDto Consent { get; set; }
        public PersonalInfoDto PersonalInfo { get; set; }
        public DemographicsDto Demographics { get; set; }
        public TechnologyDto Technology { get; set; }
        public TravelDto Travel { get; set; }
        public MediaDto Media { get; set; }
        public HealthDto Health { get; set; }
        public SurveyRegisterDto Survey { get; set; }
        public IncentivesDto Incentives { get; set; }
        public AdditionalDto Additional { get; set; }
    }

}
