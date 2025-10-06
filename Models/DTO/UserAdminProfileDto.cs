using ABC.Repositories;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ABC.Models.DTO
{
    public class UserAdminProfileDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        [Required]
        public string Email { get; set; }
        public string ContactNo { get; set; } = default!;
        public string DepartmentName { get; set; } = default!;
        public string DesignationName { get; set; } = default!;
        public string Role { get; set; } = default!;
        public List<RoleDto> Roles { get; set; }
    }
}
