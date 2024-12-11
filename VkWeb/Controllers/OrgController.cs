using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using VkCore.Constants;
using VkCore.Dtos;
using VkCore.Requests.Organization;
using VkCore.Requests.TagRequest;
using VkWeb.Extensions;

namespace VkWeb.Controllers
{
    [ApiController]
    [Authorize]
    public class OrgController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrgController(
            IMediator mediator
        )
        {
            _mediator = mediator;
        }

        #region OrgCRUD

        [HttpGet]
        [Route("api/orgs")]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            var userId = User.GetId();
            var result = await _mediator.Send(
                new GetUserOrgsRequest(userId, UserTypes.Teacher), cancellationToken);

            if (result.HasErrors())
            {
                ModelState.AddErrors(result.GetErrors());
                return new BadRequestObjectResult(ModelState);
            }

            return Ok(result.Value);
        }

        [HttpGet]
        [Route("api/org")]
        public async Task<IActionResult> Get(string id, CancellationToken cancellationToken)
        {
            var userId = User.GetId();
            var result = await _mediator.Send(
                new GetUserOrgRequest(userId, id), cancellationToken);

            if (result.HasErrors())
            {
                ModelState.AddErrors(result.GetErrors());
                return new BadRequestObjectResult(ModelState);
            }

            return Ok(result.Value);
        }

        [HttpPut]
        [Route("api/org/default")]
        public async Task<IActionResult> SetDefaultOrg(SetDefaultOrgRequest request, CancellationToken cancellationToken)
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

        #region Words

        [HttpGet]
        [Route("api/org/words")]
        public async Task<IActionResult> GetWordsProgress(string orgId, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(
                new GetWordsProgressRequest(orgId), cancellationToken);

            if (result.HasErrors())
            {
                ModelState.AddErrors(result.GetErrors());
                return new BadRequestObjectResult(ModelState);
            }

            return Ok(result.Value);
        }


        #endregion

        #region Students

        [HttpGet]
        [Route("api/org/student")]
        public async Task<IActionResult> GetStudentProgress(string orgId, string studentId, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(
                new GetStudentProgressRequest(orgId, studentId), cancellationToken);

            if (result.HasErrors())
            {
                ModelState.AddErrors(result.GetErrors());
                return new BadRequestObjectResult(ModelState);
            }

            return Ok(result.Value);
        }

        [HttpGet]
        [Route("api/org/students")]
        public async Task<IActionResult> GetStudentProgress(string orgId, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(
                new GetStudentsProgressRequest(orgId), cancellationToken);

            if (result.HasErrors())
            {
                ModelState.AddErrors(result.GetErrors());
                return new BadRequestObjectResult(ModelState);
            }

            return Ok(result.Value);
        }

        #endregion

        #region Tags

        [HttpGet]
        [Route("api/org/tag")]
        public async Task<IActionResult> GetTag(string tagId, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(
                new GetTagRequest(tagId), cancellationToken);

            if (result.HasErrors())
            {
                ModelState.AddErrors(result.GetErrors());
                return new BadRequestObjectResult(ModelState);
            }

            return Ok(result.Value);
        }

        [HttpGet]
        [Route("api/org/tag/search")]
        public async Task<IActionResult> SearchTags(string orgId, string value, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(
                new SearchOrgTagRequest(orgId, value), cancellationToken);

            if (result.HasErrors())
            {
                ModelState.AddErrors(result.GetErrors());
                return new BadRequestObjectResult(ModelState);
            }

            return Ok(result.Value);
        }

        [HttpPost]
        [Route("api/org/tag")]
        public async Task<IActionResult> AddTag(TagDto dto, CancellationToken cancellationToken)
        {
            var userId = User.GetId();
            var result = await _mediator.Send(
                new AddOrgTagRequest(dto.OrgId, userId, dto.Value), cancellationToken);

            if (result.HasErrors())
            {
                ModelState.AddErrors(result.GetErrors());
                return new BadRequestObjectResult(ModelState);
            }

            return Ok(result.Value);
        }

        [HttpPut]
        [Route("api/org/tag/default")]
        public async Task<IActionResult> SetDefaultTag(TagDto dto, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(
                new ToggleDefaultTagRequest(dto.Id), cancellationToken);

            if (result.HasErrors())
            {
                ModelState.AddErrors(result.GetErrors());
                return new BadRequestObjectResult(ModelState);
            }

            return Ok(result.Value);
        }

        [HttpDelete]
        [Route("api/org/tag")]
        public async Task<IActionResult> DeleteTag(string tagId, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new DeleteTagRequest(tagId), cancellationToken);

            if (result.HasErrors())
            {
                ModelState.AddErrors(result.GetErrors());
                return new BadRequestObjectResult(ModelState);
            }

            return Ok(result.Value);
        }

        [HttpGet]
        [Route("api/org/tag/readings")]
        public async Task<IActionResult> GetTaggedReadings(string orgId, string tagId, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(
                new GetTaggedReadingsRequest(orgId, tagId), cancellationToken);

            if (result.HasErrors())
            {
                ModelState.AddErrors(result.GetErrors());
                return new BadRequestObjectResult(ModelState);
            }

            return Ok(result.Value);
        }

        [HttpGet]
        [Route("api/org/tag/students")]
        public async Task<IActionResult> GetTaggedStudents(string orgId, string tagId, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(
                new GetTaggedStudentsRequest(orgId, tagId), cancellationToken);

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