using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using VkCore.Models;

namespace VkCore.Interfaces
{
    public interface IUserService
    {
        Task<IdentityResult> ChangePasswordAsync(VkUser user, string currentPassword, string newPassword);
        Task<IdentityResult> ConfirmEmailAsync(VkUser user, string token);
        Task<IdentityResult> CreateAsync(VkUser user, string password, CancellationToken cancellationToken);
        Task<VkUser> FindByEmailAsync(string email);
        Task<VkUser> FindByIdAsync(string id);
        Task<string> GenerateEmailConfirmationTokenAsync(VkUser user);
        Task<string> GeneratePasswordResetTokenAsync(VkUser user);
        Task<IdentityResult> ResetPasswordAsync(VkUser user, string token, string newPassword);
    }
}