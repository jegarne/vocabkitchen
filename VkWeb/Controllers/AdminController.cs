using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using VkCore.Constants;
using VkCore.Requests.Organization;
using VkCore.Requests.Student;
using VkCore.Requests.Teacher;
using VkInfrastructure.Data;
using VkWeb.Extensions;

namespace VkWeb.Controllers
{
    [ApiController]
    [Authorize]
    public class AdminController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly VkDbContext _context;
        private readonly IAuthorizationService _authorizationService;

        public AdminController(
            IMediator mediator,
            VkDbContext context,
            IAuthorizationService authorizationService
        ){
            _mediator = mediator;
            _context = context;
            _authorizationService = authorizationService;
        }

        #region Org

        [HttpGet]
        [Route("api/admin/orgs")]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            var userId = User.GetId();
            var result = await _mediator.Send(
                new GetUserOrgsRequest(userId, UserTypes.Admin), cancellationToken);

            if (result.HasErrors())
            {
                ModelState.AddErrors(result.GetErrors());
                return new BadRequestObjectResult(ModelState);
            }

            return Ok(result.Value);
        }

        [HttpGet]
        [Route("api/admin/org")]
        public async Task<IActionResult> Get(string orgId, CancellationToken cancellationToken)
        {
            var authorizationResult = await _authorizationService
                .AuthorizeAsync(User, orgId, "AdminPolicy");

            if (!authorizationResult.Succeeded)
            {
                return new ForbidResult();
            }

            var result = await _mediator.Send(new GetOrgDetailRequest(orgId));

            if (result.HasErrors())
            {
                ModelState.AddErrors(result.GetErrors());
                return new BadRequestObjectResult(ModelState);
            }
            return Ok(result.Value);
        }

        [HttpPost]
        [Route("api/admin/org")]
        public async Task<IActionResult> Post([FromBody]CreateOrgRequest query, CancellationToken cancellationtoken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _mediator.Send(query, cancellationtoken);

            if (result.HasErrors())
            {
                ModelState.AddErrors(result.GetErrors());
                return new BadRequestObjectResult(ModelState);
            }

            return Ok();
        }

        [HttpPost]
        [Route("api/admin/org/admin")]
        public async Task<IActionResult> AddAdmin([FromBody]AddOrgUserRequest request, CancellationToken cancellationtoken)
        {
            var authorizationResult = await _authorizationService
                .AuthorizeAsync(User, request.OrgId, "AdminPolicy");

            if (!authorizationResult.Succeeded)
            {
                return new ForbidResult();
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            request.UserType = UserTypes.Admin;
            var result = await _mediator.Send(request, cancellationtoken);

            if (result.HasErrors())
            {
                ModelState.AddErrors(result.GetErrors());
                return new BadRequestObjectResult(ModelState);
            }

            return Ok();
        }


        [HttpDelete]
        [Route("api/admin/org/admin")]
        public async Task<IActionResult> RemoveAdmin(string orgId, string userId, CancellationToken cancellationtoken)
        {
            var authorizationResult = await _authorizationService
                .AuthorizeAsync(User, orgId, "AdminPolicy");

            if (!authorizationResult.Succeeded)
            {
                return new ForbidResult();
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var request = new RemoveOrgUserRequest(UserTypes.Admin, orgId, userId);
            var result = await _mediator.Send(request, cancellationtoken);

            if (result.HasErrors())
            {
                ModelState.AddErrors(result.GetErrors());
                return new BadRequestObjectResult(ModelState);
            }

            return Ok();
        }

        #endregion
        #region Teachers

        [HttpGet]
        [Route("api/admin/org/teachers")]
        public async Task<IActionResult> GetOrgTeachers(string orgId, CancellationToken cancellationToken)
        {
            var authorizationResult = await _authorizationService
                .AuthorizeAsync(User, orgId, "AdminPolicy");

            if (!authorizationResult.Succeeded)
            {
                return new ForbidResult();
            }

            var userId = User.GetId();
            var result = await _mediator.Send(new GetOrgTeachersRequest(orgId, userId), cancellationToken);

            if (result.HasErrors())
            {
                ModelState.AddErrors(result.GetErrors());
                return new BadRequestObjectResult(ModelState);
            }

            return Ok(result.Value);
        }

        [HttpPost]
        [Route("api/admin/org/teachers")]
        public async Task<IActionResult> InviteTeachers([FromBody]InviteTeachersRequest request, CancellationToken cancellationtoken)
        {
            var authorizationResult = await _authorizationService
                .AuthorizeAsync(User, request.OrgId, "AdminPolicy");

            if (!authorizationResult.Succeeded)
            {
                return new ForbidResult();
            }

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

        [HttpDelete]
        [Route("api/admin/org/teacher")]
        public async Task<IActionResult> DeleteTeacher(string orgId, string teacherId, CancellationToken cancellationToken)
        {
            var authorizationResult = await _authorizationService
                .AuthorizeAsync(User, orgId, "AdminPolicy");

            if (!authorizationResult.Succeeded)
            {
                return new ForbidResult();
            }

            var result = await _mediator.Send(new RemoveOrgUserRequest(UserTypes.Teacher, orgId, teacherId), cancellationToken);

            if (result.HasErrors())
            {
                ModelState.AddErrors(result.GetErrors());
                return new BadRequestObjectResult(ModelState);
            }

            return Ok();
        }

        #endregion
        #region Students

        [HttpGet]
        [Route("api/admin/org/students")]
        public async Task<IActionResult> GetOrgStudents(string orgId, CancellationToken cancellationToken)
        {
            var authorizationResult = await _authorizationService
                .AuthorizeAsync(User, orgId, "AdminPolicy");

            if (!authorizationResult.Succeeded)
            {
                return new ForbidResult();
            }

            var result = await _mediator.Send(new GetOrgStudentsRequest(orgId), cancellationToken);

            if (result.HasErrors())
            {
                ModelState.AddErrors(result.GetErrors());
                return new BadRequestObjectResult(ModelState);
            }

            return Ok(result.Value);
        }

        [HttpPost]
        [Route("api/admin/org/students")]
        public async Task<IActionResult> InviteStudents([FromBody]InviteStudentsRequest request, CancellationToken cancellationtoken)
        {
            var authorizationResult = await _authorizationService
                .AuthorizeAsync(User, request.OrgId, "AdminPolicy");

            if (!authorizationResult.Succeeded)
            {
                return new ForbidResult();
            }

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

        [HttpDelete]
        [Route("api/admin/org/student")]
        public async Task<IActionResult> DeleteStudent(string orgId, string studentId, CancellationToken cancellationToken)
        {
            var authorizationResult = await _authorizationService
                .AuthorizeAsync(User, orgId, "AdminPolicy");

            if (!authorizationResult.Succeeded)
            {
                return new ForbidResult();
            }

            var result = await _mediator.Send(new RemoveOrgUserRequest(UserTypes.Student, orgId, studentId), cancellationToken);

            if (result.HasErrors())
            {
                ModelState.AddErrors(result.GetErrors());
                return new BadRequestObjectResult(ModelState);
            }

            return Ok();
        }

        #endregion
        #region Invites


        [HttpDelete]
        [Route("api/admin/org/invite")]
        public async Task<IActionResult> DeleteInvite(string orgId, string email, CancellationToken cancellationToken)
        {
            var authorizationResult = await _authorizationService
                .AuthorizeAsync(User, orgId, "AdminPolicy");

            if (!authorizationResult.Succeeded)
            {
                return new ForbidResult();
            }

            var result = await _mediator.Send(new RemoveOrgInviteRequest(orgId, email), cancellationToken);

            if (result.HasErrors())
            {
                ModelState.AddErrors(result.GetErrors());
                return new BadRequestObjectResult(ModelState);
            }

            return Ok();
        }

        #endregion

    }
}