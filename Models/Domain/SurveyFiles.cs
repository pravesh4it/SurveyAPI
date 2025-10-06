using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ABC.Models.Domain
{
    public class SurveyFile
    {
        public Guid Id { get; set; } = Guid.NewGuid(); // Unique file Id
        [Required]
        public Guid SurveyId { get; set; } // Foreign key to Survey
        [Required]
        [MaxLength(255)]
        public string FileName { get; set; } // Uploaded file name
        public string FileName_show { get; set; } // Uploaded file name
        public int TotalLinks { get; set; }
        public int UsedLinks { get; set; }
        public int RemainingLinks { get; set; }
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow; // Upload timestamp
        public string UploadedBy { get; set; } // User who uploaded the file
        // Navigation property
        [ForeignKey(nameof(SurveyId))]
        public virtual Survey Survey { get; set; }
    }


}
