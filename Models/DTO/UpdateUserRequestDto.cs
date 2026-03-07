namespace ABC.Models.DTO
{
    public class UpdateUserRequestDto
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Role { get; set; } // comma-separated or single
        public string DesignationId { get; set; }
        public string? ContactNo { get; set; }
    }

}
