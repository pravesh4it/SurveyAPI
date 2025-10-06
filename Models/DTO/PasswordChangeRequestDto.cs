using System.ComponentModel.DataAnnotations;

namespace ABC.Models.DTO
{
    public class PasswordChangeRequestDto
    {
        public string Id { get; set; } = string.Empty;
        public string OldPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }
}
