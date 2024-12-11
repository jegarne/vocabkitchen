using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VkCore.Events.User;
using VkCore.Extensions;
using VkCore.Interfaces;
using VkCore.Models;
using VkCore.Requests.Organization;
using VkInfrastructure.Data;

namespace VkInfrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<VkUser> _userManager;
        private readonly IMediator _mediator;
        private readonly VkDbContext _context;

        public UserService(
            UserManager<VkUser> userManager,
            IMediator mediator,
            VkDbContext context
        )
        {
            _userManager = userManager;
            _mediator = mediator;
            _context = context;
        }

        public async Task<VkUser> FindByIdAsync(string id)
        {
            return await _context.FindAsync<VkUser>(id);
        }

        public async Task<VkUser> FindByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        /// <summary>
        /// Creates a new user, accepts pending invites, and issues a NewUserAddedEvent
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IdentityResult> CreateAsync(VkUser user, string password, CancellationToken cancellationToken)
        {
            var identityResult = await _userManager.CreateAsync(user, password);

            if (!identityResult.Succeeded) return identityResult;

            var savedUser = await FindByEmailAsync(user.Email);
            var pendingInvites = _context.Invites.Where(x => x.Email == savedUser.Email).ToList();

            foreach (var invite in pendingInvites)
            {
                var accept = new AcceptInviteRequest(invite.InviteType.ToString(), invite.OrgId, savedUser.Id);
                await _mediator.Send(accept, cancellationToken);
            }

            // if there are pending invites, this request came from a user invite email
            // so assumed email is confirmed.  While this isn't foolproof, let's try it for now.
            await _mediator.Publish(new NewUserAddedEvent(savedUser, pendingInvites.Any()), cancellationToken);

            return identityResult;
        }

        public async Task<IdentityResult> ResetPasswordAsync(VkUser user, string token, string newPassword)
        {
            return await _userManager.ResetUrlFriendlyPasswordAsync(user, token, newPassword);
        }

        public async Task<IdentityResult> ConfirmEmailAsync(VkUser user, string token)
        {
            return await _userManager.ConfirmUrlFriendlyEmailAsync(user, token);
        }

        public async Task<string> GeneratePasswordResetTokenAsync(VkUser user)
        {
            return await _userManager.GenerateUrlFriendlyPasswordResetTokenAsync(user);
        }

        public async Task<string> GenerateEmailConfirmationTokenAsync(VkUser user)
        {
            return await _userManager.GenerateUrlFriendlyEmailConfirmationTokenAsync(user);
        }

        public async Task<IdentityResult> ChangePasswordAsync(VkUser user, string currentPassword, string newPassword)
        {
            return await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
        }
    }
}
