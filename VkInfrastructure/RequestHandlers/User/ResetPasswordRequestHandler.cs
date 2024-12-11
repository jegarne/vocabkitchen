using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VkCore.Interfaces;
using VkCore.Requests.User;
using VkCore.SharedKernel;
using VkInfrastructure.Extensions;

namespace VkInfrastructure.RequestHandlers.User
{
    public class ResetPasswordRequestHandler : IRequestHandler<ResetPasswordRequest, DtoResult<string>>
    {
        private readonly IUserService _userService;
        private readonly DtoResult<string> _response = new DtoResult<string>();

        public ResetPasswordRequestHandler(
            IUserService userService
        )
        {
            _userService = userService;
        }

        public async Task<DtoResult<string>> Handle(ResetPasswordRequest request, CancellationToken cancellationToken)
        {
            var vkUser = await _userService.FindByIdAsync(request.UserId);
            if (vkUser == null)
            {
                _response.AddError("User was not found.");
                return _response;
            }

            var identityResult = await _userService.ResetPasswordAsync(vkUser, request.Token, request.NewPassword);

            if (identityResult.Succeeded)
                _response.SetValue("Your password was successfully reset.");
            else
                _response.AddErrors(identityResult.GetErrors());

            return _response;
        }
    }
}
