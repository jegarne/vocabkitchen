using System.Linq;
using System.Security.Claims;
using VkInfrastructure.Auth;

namespace VkWeb.Extensions
{
    public static class ClaimsIdentityExtensions
    {
        public static string GetId(this ClaimsPrincipal principal)
        {
            return principal.Claims.FirstOrDefault(c => c.Type == JwtConstants.Strings.JwtClaimIdentifiers.Id)?.Value;
        }
    }
}
