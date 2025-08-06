using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace ABC.Models.Domain
{
    public class Client
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
        public string? C_Variable { get; set; }

        [Required]
        [MaxLength(450)]
        public string CreatedById { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        public Guid ClientTypeId { get; set; } = Guid.Empty;

        // Navigation properties for foreign key relationships
        [ForeignKey("CreatedById")]
        public virtual IdentityUser CreatedBy { get; set; }

        [ForeignKey("ClientTypeId")]
        public virtual MultiSelect ClientType { get; set; }

        [Url]
        public string? SuccessLink { get; set; } // URL if the survey is completed successfully

        [Url]
        public string? DisqualificationLink { get; set; } // URL if the respondent is disqualified
        [Url]
        public string? QuotaFullLink { get; set; } // URL if the quota is full
        [Url]
        public string? PausedLink { get; set; }
        [Url]
        public string? SecurityFailLink { get; set; }

    }

}
