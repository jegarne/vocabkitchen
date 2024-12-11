using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VkCore.Interfaces;
using VkCore.Models;
using VkCore.Requests.User;
using VkCore.SharedKernel;

namespace VkInfrastructure.RequestHandlers.User
{
    public class GetUserDetailsRequestHandler : IRequestHandler<GetUserDetailsRequest, DtoResult<VkUser>>
    {
        private readonly IUserService _userService;
        private readonly DtoResult<VkUser> _response = new DtoResult<VkUser>();

        public GetUserDetailsRequestHandler(
            IUserService userService
        )
        {
            _userService = userService;
        }

        public async Task<DtoResult<VkUser>> Handle(GetUserDetailsRequest request, CancellationToken cancellationToken)
        {
            if (request.UserId == null)
            {
                _response.AddError("Please log in.");
                return _response;
            }
            var vkUser = await _userService.FindByIdAsync(request.UserId);
            if (vkUser == null)
            {
                _response.AddError("User was not found.");
                return _response;
            }

            _response.SetValue(vkUser);

            return _response;
        }
    }
}
