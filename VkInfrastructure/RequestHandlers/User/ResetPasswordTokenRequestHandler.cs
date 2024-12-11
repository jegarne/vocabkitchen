using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using VkCore.Interfaces;
using VkCore.Requests.User;
using VkCore.SharedKernel;
using VkWeb.Extensions;

namespace VkInfrastructure.RequestHandlers.User
{
    public class ResetPasswordTokenRequestHandler : IRequestHandler<ResetPasswordTokenRequest, DtoResult<string>>
    {
        private readonly IUserService _userService;
        private readonly IEmailSender _emailSender;
        private readonly IHttpContextAccessor _context;
        private readonly DtoResult<string> _response = new DtoResult<string>();

        public ResetPasswordTokenRequestHandler(
            IUserService userService,
            IEmailSender emailSender,
            IHttpContextAccessor context
        )
        {
            _userService = userService;
            _emailSender = emailSender;
            _context = context;
        }

        public async Task<DtoResult<string>> Handle(ResetPasswordTokenRequest requestRequest, CancellationToken cancellationToken)
        {
            if (requestRequest.Email == null)
            {
                _response.AddError("Please enter an email.");
                return _response;
            }

            var user = await _userService.FindByEmailAsync(requestRequest.Email);
            if (user == null)
            {
                _response.AddError("Email not found.");
                return _response;
            }

            var token = await _userService.GeneratePasswordResetTokenAsync(user);
            var callbackUrl = $"{_context.GetBaseUrl()}/reset-password?userId={user.Id}&token={token}";
            await _emailSender.SendEmailAsync(user.Email, "Reset your VocabKitchen password",
                $"Please reset your password by <a href='{callbackUrl}'>clicking here</a>.");

            _response.SetValue("Password reset email sent.");
            return _response;
        }
    }
}
