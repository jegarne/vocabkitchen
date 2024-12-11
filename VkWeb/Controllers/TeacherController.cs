using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using VkCore.Requests.Teacher;
using VkWeb.Extensions;

namespace VkWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TeacherController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]InviteTeachersRequest request, CancellationToken cancellationtoken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _mediator.Send(request, cancellationtoken);

            if (result.HasErrors())
            {
                ModelState.AddErrors(result.GetErrors());
                return new BadRequestObjectResult(ModelState);
            }

            return Ok(new { value = result.Value });
        }
    }
}