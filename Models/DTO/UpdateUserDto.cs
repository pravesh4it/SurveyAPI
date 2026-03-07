using System.ComponentModel.DataAnnotations;

namespace ABC.Models.DTO
{
    public class UpdateUserDto
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string RoleId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public Guid DesignationId { get; set; }
        public string ContactNo { get; set; }

    }
}
