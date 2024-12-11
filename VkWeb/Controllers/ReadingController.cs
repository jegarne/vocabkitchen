using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using VkCore.Requests.Organization;
using VkCore.Requests.ReadingRequest;
using VkCore.Requests.TagRequest;
using VkWeb.Extensions;

namespace VkWeb.Controllers
{
    [ApiController]
    [Authorize]
    public class ReadingController : ControllerBase
    {
        private readonly IMediator _mediator;
           private readonly IAuthorizationService _authorizationService;

        public ReadingController(
            IMediator mediator,
            IAuthorizationService authorizationService
        )
        {
            _mediator = mediator;
            _authorizationService = authorizationService;
        }

        // TODO: lock down with auth service

        #region ReadingCRUD

        [HttpGet("api/reading")]
        public async Task<IActionResult> Get(string id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetReadingRequest(id, User.GetId()), cancellationToken);

            if (result.HasErrors())
            {
                ModelState.AddErrors(result.GetErrors());
                return new BadRequestObjectResult(ModelState);
            }

            return Ok(result.Value);
        }

        [HttpPost("api/reading")]
        public async Task<IActionResult> Post(AddOrgReadingRequest request, CancellationToken cancellationToken)
        {
            request.UserId = User.GetId();
            var result = await _mediator.Send(request, cancellationToken);

            if (result.HasErrors())
            {
                ModelState.AddErrors(result.GetErrors());
                return new BadRequestObjectResult(ModelState);
            }

            return Ok(result.Value);
        }

        [HttpDelete("api/reading")]
        public async Task<IActionResult> Delete(string id, CancellationToken cancellationToken)
        {
            var request = new DeleteReadingRequest(id);
            var result = await _mediator.Send(request, cancellationToken);

            if (result.HasErrors())
            {
                ModelState.AddErrors(result.GetErrors());
                return new BadRequestObjectResult(ModelState);
            }

            return Ok();
        }

        [HttpPost("api/reading/profiled")]
        public async Task<IActionResult> PostFr(AddProfilerReadingRequest request, CancellationToken cancellationToken)
        {
            request.UserId = User.GetId();
            var result = await _mediator.Send(request, cancellationToken);

            if (result.HasErrors())
            {
                ModelState.AddErrors(result.GetErrors());
                return new BadRequestObjectResult(ModelState);
            }

            return Ok(result.Value);
        }

        [HttpPut("api/reading")]
        public async Task<IActionResult> Put(UpdateReadingRequest request, CancellationToken cancellationToken)
        {
            request.UserId = User.GetId();
            var result = await _mediator.Send(request, cancellationToken);

            if (result.HasErrors())
            {
                ModelState.AddErrors(result.GetErrors());
                return new BadRequestObjectResult(ModelState);
            }

            return Ok(result.Value);
        }

        #endregion

        #region Definitions

        [HttpPost("api/reading/definition")]
        public async Task<IActionResult> AddDefinition(AddDefinitionRequest request, CancellationToken cancellationToken)
        {
            request.UserId = User.GetId();
            var result = await _mediator.Send(request, cancellationToken);

            if (result.HasErrors())
            {
                ModelState.AddErrors(result.GetErrors());
                return new BadRequestObjectResult(ModelState);
            }

            return Ok(result.Value);
        }

        [HttpPut("api/reading/definition")]
        public async Task<IActionResult> UpdateDefinition(UpdateDefinitionRequest request, CancellationToken cancellationToken)
        {
            request.UserId = User.GetId();
            var result = await _mediator.Send(request, cancellationToken);

            if (result.HasErrors())
            {
                ModelState.AddErrors(result.GetErrors());
                return new BadRequestObjectResult(ModelState);
            }

            return Ok();
        }

        [HttpDelete("api/reading/definition")]
        public async Task<IActionResult> RemoveDefinition(string readingId, string contentItemId, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new RemoveDefinitionRequest(readingId, contentItemId), cancellationToken);

            if (result.HasErrors())
            {
                ModelState.AddErrors(result.GetErrors());
                return new BadRequestObjectResult(ModelState);
            }

            return Ok(result.Value);
        }

        #endregion

        #region Tags

        [HttpPost("api/reading/tag")]
        public async Task<IActionResult> PostTag(AddReadingTagRequest request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(request, cancellationToken);

            if (result.HasErrors())
            {
                ModelState.AddErrors(result.GetErrors());
                return new BadRequestObjectResult(ModelState);
            }

            return Ok(result.Value);
        }

        [HttpDelete("api/reading/tag")]
        public async Task<IActionResult> DeleteTag(string id, string tagId, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new RemoveReadingTagRequest(id, tagId), cancellationToken);

            if (result.HasErrors())
            {
                ModelState.AddErrors(result.GetErrors());
                return new BadRequestObjectResult(ModelState);
            }

            return Ok(result.Value);
        }

        [HttpPost("api/reading/tags")]
        public async Task<IActionResult> MergeTags(MergeReadingTagsRequest request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(request, cancellationToken);

            if (result.HasErrors())
            {
                ModelState.AddErrors(result.GetErrors());
                return new BadRequestObjectResult(ModelState);
            }

            return Ok(result.Value);
        }

        #endregion
    }
}