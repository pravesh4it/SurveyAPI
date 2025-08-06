using ABC.Models.DTO;
using Microsoft.AspNetCore.Identity;

namespace ABC.Models.Domain
{
    public class ApplicationUser : IdentityUser
    {
        public virtual UserProfile Profile { get; set; }
    }
}
