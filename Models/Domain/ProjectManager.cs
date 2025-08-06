using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace ABC.Models.Domain
{
    public class ProjectManager
    {
        public Guid Id { get; set; } // Primary Key
        public string UserId { get; set; } // IdentityUser ID
        [ForeignKey("UserId")]
        public virtual IdentityUser User { get; set; } // Relationship with IdentityUser
        public Guid SurveyId { get; set; } // Foreign Key for Survey
        [ForeignKey("SurveyId")]
        public virtual Survey Survey { get; set; } // Navigation Property
    }
}
