using ABC.Models.Domain;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ABC.Models.DTO
{
    public class ClientDto
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required]
        [MaxLength(255)]
        public string ContactPerson { get; set; }

        [Required]
        [MaxLength(500)]
        public string Address { get; set; }

        [MaxLength(15)]
        public string ContactNo1 { get; set; }

        [Required]
        [MaxLength(255)]
        public string Email { get; set; }

        [MaxLength(255)]
        public string C_Variable { get; set; }

        [Required]
        [MaxLength(450)]
        public string CreatedById { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ClientTypeId { get; set; }
        // Newly added fields
        [MaxLength(500)]
        public string? DisqualificationLink { get; set; }

        [MaxLength(500)]
        public string? PausedLink { get; set; }

        [MaxLength(500)]
        public string? QuotaFullLink { get; set; }

        [MaxLength(500)]
        public string? SecurityFailLink { get; set; }

        [MaxLength(500)]
        public string? SuccessLink { get; set; }

    }
}
