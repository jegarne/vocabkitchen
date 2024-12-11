using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using VkCore.Requests.Student;
using VkCore.Requests.TagRequest;
using VkWeb.Extensions;

namespace VkWeb.Controllers
{
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StudentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("api/student")]
        public async Task<IActionResult> Get(CancellationToken cancellationtoken, string id = null)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (string.IsNullOrWhiteSpace(id))
                id = User.GetId();

            var result = await _mediator.Send(new GetStudentDetailsRequest(id), cancellationtoken);

            if (result.HasErrors())
            {
                ModelState.AddErrors(result.GetErrors());
                return new BadRequestObjectResult(ModelState);
            }

            return Ok(result.Value);
        }

        [HttpPost("api/student")]
        public async Task<IActionResult> Post([FromBody]InviteStudentsRequest request, CancellationToken cancellationtoken)
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

        [HttpGet("api/student/readings")]
        public async Task<IActionResult> GetReadings(CancellationToken cancellationtoken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _mediator.Send(new GetStudentReadingsRequest(User.GetId()), cancellationtoken);

            if (result.HasErrors())
            {
                ModelState.AddErrors(result.GetErrors());
                return new BadRequestObjectResult(ModelState);
            }

            return Ok(result.Value);
        }

        #region Words

        [HttpGet("api/student/words")]
        public async Task<IActionResult> GetWords(CancellationToken cancellationtoken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _mediator.Send(new GetStudentWordsRequest(User.GetId()), cancellationtoken);

            if (result.HasErrors())
            {
                ModelState.AddErrors(result.GetErrors());
                return new BadRequestObjectResult(ModelState);
            }

            return Ok(result.Value);
        }

        [HttpPost("api/student/word")]
        public async Task<IActionResult> AddUnkownWord(AddStudentWordRequest request, CancellationToken cancellationtoken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            request.UserId = User.GetId();
            var result = await _mediator.Send(request, cancellationtoken);

            if (result.HasErrors())
            {
                ModelState.AddErrors(result.GetErrors());
                return new BadRequestObjectResult(ModelState);
            }

            return Ok(result.Value);
        }

        [HttpGet("api/student/pretest")]
        public async Task<IActionResult> GetPretestWords(string readingId, CancellationToken cancellationtoken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _mediator.Send(new GetPretestWordsRequest(User.GetId(), readingId), cancellationtoken);

            if (result.HasErrors())
            {
                ModelState.AddErrors(result.GetErrors());
                return new BadRequestObjectResult(ModelState);
            }

            return Ok(result.Value);
        }

        [HttpGet("api/student/words-unknown")]
        public async Task<IActionResult> GetUnknownWords(string readingId, CancellationToken cancellationtoken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _mediator.Send(new GetUnkownWordsRequest(User.GetId(), readingId), cancellationtoken);

            if (result.HasErrors())
            {
                ModelState.AddErrors(result.GetErrors());
                return new BadRequestObjectResult(ModelState);
            }

            return Ok(result.Value);
        }

        [HttpGet("api/student/words-random")]
        public async Task<IActionResult> GetRandomWords(CancellationToken cancellationtoken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _mediator.Send(new GetDistractorListRequest(), cancellationtoken);

            if (result.HasErrors())
            {
                ModelState.AddErrors(result.GetErrors());
                return new BadRequestObjectResult(ModelState);
            }

            return Ok(result.Value);
        }

        [HttpPost("api/student/word-known")]
        public async Task<IActionResult> MarkWordKnown(AddKnownWordRequest request, CancellationToken cancellationtoken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            request.UserId = User.GetId();
            var result = await _mediator.Send(request, cancellationtoken);

            if (result.HasErrors())
            {
                ModelState.AddErrors(result.GetErrors());
                return new BadRequestObjectResult(ModelState);
            }

            return Ok(result.Value);
        }

        [HttpPost("api/student/word-attempt")]
        public async Task<IActionResult> AddAttempt(AddWordAttemptRequest request, CancellationToken cancellationtoken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            request.UserId = User.GetId();
            var result = await _mediator.Send(request, cancellationtoken);

            if (result.HasErrors())
            {
                ModelState.AddErrors(result.GetErrors());
                return new BadRequestObjectResult(ModelState);
            }

            return Ok(result.Value);
        }

        #endregion

        #region Tags

        [HttpPost("api/student/tag")]
        public async Task<IActionResult> PostTag(AddStudentTagRequest request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(request, cancellationToken);

            if (result.HasErrors())
            {
                ModelState.AddErrors(result.GetErrors());
                return new BadRequestObjectResult(ModelState);
            }

            return Ok(result.Value);
        }

        [HttpDelete("api/student/tag")]
        public async Task<IActionResult> DeleteTag(string id, string tagId, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new RemoveStudentTagRequest(id, tagId), cancellationToken);

            if (result.HasErrors())
            {
                ModelState.AddErrors(result.GetErrors());
                return new BadRequestObjectResult(ModelState);
            }

            return Ok(result.Value);
        }

        [HttpPost("api/student/tags")]
        public async Task<IActionResult> MergeTags(MergeStudentTagsRequest request, CancellationToken cancellationToken)
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