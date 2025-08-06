using System.ComponentModel.DataAnnotations;

namespace ABC.Models.DTO
{
    public class LoginRequestDto
    {
        [Required]
        [EmailAddress]
        public string Username { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }


            
    }
}
