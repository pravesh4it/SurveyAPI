using System.ComponentModel.DataAnnotations;

namespace ABC.Models.DTO
{
    public class RegisterRequestDto
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Username { get; set; }        
        [Required]
        public string Role { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public Guid DesignationId { get; set; }
        public string ContactNo { get; set; }

    }
}
