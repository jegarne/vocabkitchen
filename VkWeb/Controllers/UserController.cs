using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using VkCore.Requests.User;
using VkWeb.Extensions;

namespace VkWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetUserDetailsRequest(User.GetId()), cancellationToken);

            if (result.HasErrors())
            {
                ModelState.AddErrors(result.GetErrors());
                return new BadRequestObjectResult(ModelState);
            }

            return Ok(result.Value);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]CreateUserRequest request, CancellationToken cancellationtoken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _mediator.Send(request, cancellationtoken);

            if (result.HasErrors())
            {
                ModelState.AddErrors(result.GetErrors());
                return new BadRequestObjectResult(ModelState);
            }

            return Ok(result.Value);
        }


        [HttpPost("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequest request, CancellationToken cancellationToken)
        {
            request.UserId = User.GetId();
            var result = await _mediator.Send(request, cancellationToken);

            if (result.HasErrors())
            {
                ModelState.AddErrors(result.GetErrors());
                return new BadRequestObjectResult(ModelState);
            }

            return Ok(new { value = result.Value });
        }
    }
}