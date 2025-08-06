using ABC.Models.Domain;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ABC.Models.DTO
{
    public class SurveyDto
    {
        public Guid Id { get; set; } // Primary Key
        public string Name { get; set; }
        public string Language { get; set; }
        public string Country { get; set; }
        public string Status { get; set; } // Enum for Status
        public string Client { get; set; }
        public string Title { get; set; }
        public DateTime LaunchedDate { get; set; }
        public int SurveyQuota { get; set; }
        public int RequiredQuota { get; set; }
        public int CurrentComplete { get; set; }
        public decimal CPI { get; set; }
        public int VendorsCount { get; set; }
        public int CloneCount { get; set; }

    }
}
