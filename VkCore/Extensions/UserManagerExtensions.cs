using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using VkCore.Models;

namespace VkCore.Extensions
{
    public static class UserManagerExtensions
    {
        public static async Task<string> GenerateUrlFriendlyEmailConfirmationTokenAsync(this UserManager<VkUser> manager, VkUser user)
        {
            var token = await manager.GenerateEmailConfirmationTokenAsync(user);
            byte[] tokenGeneratedBytes = Encoding.UTF8.GetBytes(token);
            return WebEncoders.Base64UrlEncode(tokenGeneratedBytes);
        }

        public static async Task<IdentityResult> ConfirmUrlFriendlyEmailAsync(this UserManager<VkUser> manager, VkUser user, string token)
        {
            var tokenDecodedBytes = WebEncoders.Base64UrlDecode(token);
            var tokenDecoded = Encoding.UTF8.GetString(tokenDecodedBytes);
            return await manager.ConfirmEmailAsync(user, tokenDecoded);
        }

        public static async Task<string> GenerateUrlFriendlyPasswordResetTokenAsync(this UserManager<VkUser> manager, VkUser user)
        {
            var token = await manager.GeneratePasswordResetTokenAsync(user);
            byte[] tokenGeneratedBytes = Encoding.UTF8.GetBytes(token);
            return WebEncoders.Base64UrlEncode(tokenGeneratedBytes);
        }

        public static async Task<IdentityResult> ResetUrlFriendlyPasswordAsync(this UserManager<VkUser> manager, VkUser user, string token, string newPassword)
        {
            var tokenDecodedBytes = WebEncoders.Base64UrlDecode(token);
            var tokenDecoded = Encoding.UTF8.GetString(tokenDecodedBytes);
            return await manager.ResetPasswordAsync(user, tokenDecoded, newPassword);
        }
    }
}
