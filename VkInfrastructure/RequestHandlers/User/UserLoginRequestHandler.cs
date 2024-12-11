using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using VkCore.Models;
using VkCore.Requests.User;
using VkCore.SharedKernel;
using VkInfrastructure.Auth;

namespace VkInfrastructure.RequestHandlers.User
{
    public class UserLoginRequestHandler : IRequestHandler<UserLoginRequest, DtoResult<string>>
    {
        private readonly UserManager<VkUser> _userManager;
        private readonly IJwtFactory _jwtFactory;
        private readonly DtoResult<string> _response = new DtoResult<string>();

        public UserLoginRequestHandler(
            UserManager<VkUser> userManager,
            IJwtFactory jwtFactory
        )
        {
            _userManager = userManager;
            _jwtFactory = jwtFactory;
        }

        public async Task<DtoResult<string>> Handle(UserLoginRequest dto, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(dto.UserName) || string.IsNullOrEmpty(dto.Password))
            {
                _response.AddError("Please enter a username and password.");
                return _response;
            }

            // get the user to verifty
            var userToVerify = await _userManager.FindByNameAsync(dto.UserName);
            if (userToVerify == null)
            {
                _response.AddError("No user was found with that username.");
                return _response;
            }

            if (!_userManager.IsEmailConfirmedAsync(userToVerify).Result)
            {
                _response.AddError("Please confirm your email first. Check your email for a confirmation link."
                                    + "<br /><br />Didn't receive an email?  " +
                                    "<a href=\"/request-email-confirmation\">Request a new one</a>");
                return _response;
            }

            // check the credentials
            if (!await _userManager.CheckPasswordAsync(userToVerify, dto.Password))
            {
                // Credentials are invalid, or account doesn't exist
                _response.AddError("Invalid username and password combination.");
                return _response;
            }

            // var identity = _jwtFactory.GenerateClaimsIdentity(dto.UserName, userToVerify.Id);
            var jwt = await _jwtFactory.GenerateJwt(dto.UserName, userToVerify.Id);
            _response.SetValue(jwt);

            return _response;
        }
    }
}
