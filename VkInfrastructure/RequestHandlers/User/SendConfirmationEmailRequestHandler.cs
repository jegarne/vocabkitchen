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
    public class SendConfirmationEmailRequestHandler : IRequestHandler<SendConfirmationEmailRequest, DtoResult<string>>
    {
        private readonly IUserService _userService;
        private readonly IEmailSender _emailSender;
        private readonly IHttpContextAccessor _context;
        private readonly DtoResult<string> _response = new DtoResult<string>();

        public SendConfirmationEmailRequestHandler(
            IUserService userService,
            IEmailSender emailSender,
            IHttpContextAccessor context
        )
        {
            _userService = userService;
            _emailSender = emailSender;
            _context = context;
        }

        public async Task<DtoResult<string>> Handle(SendConfirmationEmailRequest request, CancellationToken cancellationToken)
        {
            var user = await _userService.FindByEmailAsync(request.Email);
            if (user == null)
            {
                _response.AddError("Email not found.");
                return _response;
            }

            var token = await _userService.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = $"{_context.GetBaseUrl()}/api/auth/confirm-email?userid={user.Id}&token={token}";       
            await _emailSender.SendEmailAsync(user.Email, "VocabKitchen - confirm your email",
                $"Please confirm your account by <a href='{callbackUrl}'>clicking here</a>.");

            _response.SetValue("Confirmation email sent.");
            return _response;
        }
    }
}
