using MediatR;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VkCore.Models.Profiler;
using VkCore.Requests.Profiler;

namespace VkWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfilerController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProfilerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Profile(ProfilerRequest request, CancellationToken cancellationtoken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _mediator.Send(request, cancellationtoken);

            return Ok(result);
        }

        [HttpPost("/api/profiler/word-doc")]
        public async Task<IActionResult> GetWordDoc(ProfilerRequest request, CancellationToken cancellationtoken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _mediator.Send(request, cancellationtoken);

            string docContent = result.GetCefrDoc();
            byte[] buffer = Encoding.Unicode.GetPreamble()
                .Concat(Encoding.Unicode.GetBytes(docContent))
                .ToArray();

            string docName = $"VocabProfile {DateTime.Now.ToShortDateString()} {DateTime.Now.ToShortTimeString()}.doc";
            docName = docName.Replace(" ", "-");
            docName = docName.Replace(":", "-");
            docName = docName.Replace("/", "-");

            return File(buffer, "application/msword", docName);
        }
    }
}