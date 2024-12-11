using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VkCore.Dtos;
using VkCore.Interfaces;
using VkCore.Models;
using VkCore.Requests.Organization;
using VkCore.Requests.User;
using VkCore.SharedKernel;
using VkInfrastructure.Extensions;

namespace VkInfrastructure.RequestHandlers.User
{
    public class CreateUserRequestHandler : IRequestHandler<CreateUserRequest, DtoResult<UserDto>>
    {
        private readonly IUserService _userService;
        private readonly IMediator _mediator;
        private readonly DtoResult<UserDto> _response;

        public CreateUserRequestHandler(IUserService userService, IMediator mediator)
        {
            _mediator = mediator;
            _userService = userService;
            _response = new DtoResult<UserDto>();
        }

        public async Task<DtoResult<UserDto>> Handle(CreateUserRequest request, CancellationToken cancellationToken)
        {
            var existingUser = await _userService.FindByEmailAsync(request.Email);

            if(existingUser != null)
            {
                _response.AddError($"A user with email {request.Email} already exists.");
                return _response;
            }

            var newUser = request.ToVkUser();
            var identityResult = await _userService.CreateAsync(newUser, request.Password, cancellationToken);
            if (!identityResult.Succeeded)
            {
                _response.AddErrors(identityResult.GetErrors());
                return _response;
            };

            if (_response.HasErrors()) return _response;
            var savedUser = await _userService.FindByEmailAsync(newUser.Email);
            _response.SetValue(new UserDto(savedUser));

            if (!string.IsNullOrWhiteSpace(request.OrganizationName))
            {
                var createOrgRequest = new CreateOrgRequest(savedUser.Id, request.OrganizationName);
                var newOrgResult = await _mediator.Send(createOrgRequest);
                _response.SetValue(new UserDto(savedUser, newOrgResult.Value));
            }

            return _response;
        }
    }
}
