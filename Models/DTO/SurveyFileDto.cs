using ABC.Models.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ABC.Models.DTO
{
    public class SurveyFileDto
    {
        [Required]
        public Guid SurveyId { get; set; } // Foreign key to Survey
        public string? FileName { get; set; } // Uploaded file name
        public string UploadedBy { get; set; } // User who uploaded the file
        [Required]
        public IFormFile File { get; set; }

    }
}
