using Microsoft.AspNetCore.Identity;

namespace ThangAPI.Repositoty
{
    public interface ITokenRepository
    {
        string CreateJWTToken(IdentityUser user, List<string> roles);
    }
}
