using System.Security.Claims;
using System.Threading.Tasks;

namespace VkInfrastructure.Auth
{
    public interface IJwtFactory
    {
        //Task<string> GenerateEncodedToken(string userName, ClaimsIdentity identity);
        //ClaimsIdentity GenerateClaimsIdentity(string userName, string id);
        Task<string> GenerateJwt(string userName, string userId);
    }
}
