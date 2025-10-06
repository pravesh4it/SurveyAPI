using System.ComponentModel.DataAnnotations;

namespace ABC.Models.DTO
{
    public record MultiselectReadDto(
        Guid Id,
        string Name,
        string DisplayName,
        bool IsActive,
        string? Description,
        string SelectionType
        );


    public class MultiselectCreateDto
    {
        [Required, MaxLength(150)] public string Name { get; set; } = string.Empty;
        [Required, MaxLength(200)] public string DisplayName { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        [MaxLength(1000)] public string? Description { get; set; }
        [Required, MaxLength(100)] public string SelectionType { get; set; } = string.Empty;
    }


    public class MultiselectUpdateDto : MultiselectCreateDto { }
}
