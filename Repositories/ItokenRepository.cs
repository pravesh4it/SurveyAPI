using Microsoft.AspNetCore.Identity;

namespace ABC.Repositories
{
    public interface ItokenRepository
    {
        string CreateJWTToken(IdentityUser user, List<string> roles);
    }
}
