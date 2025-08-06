using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace ABC.Models.Domain
{
    public class UserInfo
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string AspNetUsersId { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        [Required]
        public Guid DepartmentId { get; set; }

        [Required]
        public string UserTypeId { get; set; }

        public string ContactNo { get; set; }
        public bool IsFirstLogin { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime LastModifiedDate { get; set; }

        public string LastModifiedBy { get; set; }
        public string? PasswordResetCode { get; set; }
        public DateTime? ResetCodeSentDate { get; set; }

        // Navigation property if there's a relationship with AspNetUsers
        [ForeignKey("AspNetUsersId")]
        public virtual IdentityUser AspNetUser { get; set; }

        // Navigation property if there's a relationship with Department
        [ForeignKey("DepartmentId")]
        public virtual Department Department { get; set; }
        [Required]
        public Guid DesignationId { get; set; }
        // Navigation property if there's a relationship with Department
        [ForeignKey("DesignationId")]
        public virtual Designation Designation { get; set; }


    }
}
