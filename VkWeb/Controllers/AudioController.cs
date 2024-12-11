using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VkCore.Requests.Word;
using VkInfrastructure.Data;

namespace VkWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AudioController : ControllerBase
    {
        private readonly VkDbContext _context;
        private readonly IMediator _mediator;

        public AudioController(VkDbContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            var words = _context.Words.Select(w => w.Word);

            foreach (var word in words)
            {
                await _mediator.Send(new CreateWordAudioRequest(word), cancellationToken);
            }

            return Ok();
        }

    }
}