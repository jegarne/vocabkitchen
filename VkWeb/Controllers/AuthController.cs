using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using VkCore.Requests.User;
using VkWeb.Extensions;

namespace VkWeb.Controllers
{
    // api/auth
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]UserLoginRequest request, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _mediator.Send(request, cancellationToken);

            if (result.HasErrors())
            {
                ModelState.AddErrors(result.GetErrors());
                return new BadRequestObjectResult(ModelState);
            }

            return Ok(result.Value);
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new ConfirmEmailRequest(userId, token), cancellationToken);

            if (result.HasErrors())
            {
                ModelState.AddErrors(result.GetErrors());
                return new BadRequestObjectResult(ModelState);
            }

            return Ok(result.Value);
        }

        [HttpGet("resend-confirmation-email")]
        public async Task<IActionResult> ResendConfirmationEmail(string email, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new SendConfirmationEmailRequest(email), cancellationToken);

            if (result.HasErrors())
            {
                ModelState.AddErrors(result.GetErrors());
                return new BadRequestObjectResult(ModelState);
            }

            return Ok();
        }

        [HttpGet("request-password-reset")]
        public async Task<IActionResult> RequestPasswordReset(string email, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new ResetPasswordTokenRequest(email), cancellationToken);

            if (result.HasErrors())
            {
                ModelState.AddErrors(result.GetErrors());
                return new BadRequestObjectResult(ModelState);
            }

            return Ok(new { value = result.Value });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(request, cancellationToken);

            if (result.HasErrors())
            {
                ModelState.AddErrors(result.GetErrors());
                return new BadRequestObjectResult(ModelState);
            }

            return Ok(new { value = result.Value});
        }

        [HttpGet("access")]
        [Authorize]
        public async Task<IActionResult> GetAccess(CancellationToken cancellationToken)
        {
            var request = new UserAccessRequest(User.GetId());
            var result = await _mediator.Send(request, cancellationToken);

            if (result.HasErrors())
            {
                ModelState.AddErrors(result.GetErrors());
                return new BadRequestObjectResult(ModelState);
            }

            return Ok(result.Value);
        }
    }
}