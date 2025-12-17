using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ABC.Models.Domain
{
    public class Survey
    {
        [Key]
        public Guid Id { get; set; } // Primary Key

        // Self-referencing foreign key
        public int AutoNumber { get; set; }   // New auto-increment-like field
        public Guid? ParentId { get; set; }

        [ForeignKey("ParentId")]
        public virtual Survey ParentSurvey { get; set; }

        public string Title { get; set; }
        public string Name { get; set; }
        public string Language { get; set; }
        public string Country { get; set; }
        public int FilledTimeInDays { get; set; }
        public int? Completes { get; set; }
        public int LengthOfSurveyInMinutes { get; set; }
        public decimal? Incidence { get; set; }
        public string Status { get; set; } // Enum for Status
        public string ClientOrPartner { get; set; }
        public string ClientLink { get; set; }

        public string CreatedById { get; set; }

        [ForeignKey("CreatedById")]
        public virtual IdentityUser CreatedBy { get; set; }

        public virtual ICollection<ProjectManager> ProjectManagers { get; set; }
        public virtual ICollection<SalesManager> SalesManagers { get; set; }

        [Url]
        public string Success_Link { get; set; }

        [Url]
        public string Disqualification_Link { get; set; }

        [Url]
        public string QuotaFull_Link { get; set; }

        public DateOnly LaunchedDate { get; set; }
        public DateOnly? EndDate { get; set; }

        public string CurrencyId { get; set; }
        public decimal ClientIR { get; set; }
        public decimal ClientRate { get; set; }
        public int SurveyQuota { get; set; }
        public bool PreScreener { get; set; }
        public bool UniqueLink { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedOn { get; set; }= DateTime.UtcNow; // Record creation timestamp

        // Navigation for children (optional)
        public virtual ICollection<Survey> ClonedSurveys { get; set; }

        public Survey()
        {
            ProjectManagers = new List<ProjectManager>();
            SalesManagers = new List<SalesManager>();
            ClonedSurveys = new List<Survey>();
        }
    }

}
