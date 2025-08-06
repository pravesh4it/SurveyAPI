namespace ABC.Models.DTO
{
    public class PersonalInfoDto
    {
        public string Name { get; set; }
        public string Email { get; set; } // Consider validation
        public string Phone { get; set; }
        public DateTime Dob { get; set; }
        public string Gender { get; set; }
        public string Country { get; set; }
        public string Zip { get; set; }
        public string Income { get; set; }
    }
}
