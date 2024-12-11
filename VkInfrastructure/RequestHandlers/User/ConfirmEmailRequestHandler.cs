using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VkCore.Interfaces;
using VkCore.Requests.User;
using VkCore.SharedKernel;
using VkInfrastructure.Extensions;

namespace VkInfrastructure.RequestHandlers.User
{
    public class ConfirmEmailRequestHandler : IRequestHandler<ConfirmEmailRequest, DtoResult<string>>
    {
        private readonly IUserService _userService;
        private readonly DtoResult<string> _response = new DtoResult<string>();

        public ConfirmEmailRequestHandler(
            IUserService userService
        )
        {
            _userService = userService;
        }

        public async Task<DtoResult<string>> Handle(ConfirmEmailRequest request, CancellationToken cancellationToken)
        {
            var vkUser = await _userService.FindByIdAsync(request.UserId);
            if (vkUser == null)
            {
                _response.AddError("User was not found.");
                return _response;
            }

            var identityResult = await _userService.ConfirmEmailAsync(vkUser, request.Token);

            if (identityResult.Succeeded)
                _response.SetValue("Thanks for signing up at vocabkitchen.com.  Your email was successfully confirmed.  Please log in and try it out.");
            else
                _response.AddErrors(identityResult.GetErrors());

            return _response;
        }
    }
}
