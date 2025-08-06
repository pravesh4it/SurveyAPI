namespace ABC.Models.DTO
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Additional
    {
        public string referral { get; set; }
    }

    public class Consent
    {
        public bool terms { get; set; }
        public bool privacyPolicy { get; set; }
        public bool dataProcessing { get; set; }
        public bool newsletter { get; set; }
    }

    public class Demographics
    {
        public string maritalStatus { get; set; }
        public string children { get; set; }
        public string householdSize { get; set; }
        public string educationLevel { get; set; }
        public string employmentStatus { get; set; }
    }

    public class Health
    {
        public string status { get; set; }
        public string chronic { get; set; }
        public string smokingAlcohol { get; set; }
        public string exercise { get; set; }
        public string diet { get; set; }
    }

    public class Incentives
    {
        public string reward { get; set; }
        public string frequency { get; set; }
    }

    public class Media
    {
        public List<string> platforms { get; set; }
    }

    public class PersonalInfo
    {
        public string name { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string dob { get; set; }
        public string gender { get; set; }
        public string country { get; set; }
        public string zip { get; set; }
        public string income { get; set; }
    }

    public class UserProfile
    {
        public Consent consent { get; set; }
        public PersonalInfo personalInfo { get; set; }
        public Demographics demographics { get; set; }
        public Technology technology { get; set; }
        public Travel travel { get; set; }
        public Media media { get; set; }
        public Health health { get; set; }
        public Survey survey { get; set; }
        public Incentives incentives { get; set; }
        public Additional additional { get; set; }
    }

    public class Survey
    {
        public string length { get; set; }
        public List<string> formats { get; set; }
        public string frequency { get; set; }
    }

    public class Technology
    {
        public List<string> devices { get; set; }
        public string shoppingFrequency { get; set; }
    }

    public class Travel
    {
        public string domestic { get; set; }
        public string international { get; set; }
    }


}
