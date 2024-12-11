using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using VkCore.Models.Organization;
using VkCore.Requests.Teacher;
using VkCore.SharedKernel;
using VkInfrastructure.Data;
using VkWeb.Extensions;

namespace VkInfrastructure.RequestHandlers.Teacher
{
    public class InviteTeachersRequestHandler : IRequestHandler<InviteTeachersRequest, DtoResult<string>>
    {
        private readonly VkDbContext _context;
        private readonly DtoResult<string> _response = new DtoResult<string>();
        private readonly IEmailSender _emailSender;
        private readonly IHttpContextAccessor _httpContext;

        public InviteTeachersRequestHandler(
            VkDbContext context,
            IEmailSender emailSender,
            IHttpContextAccessor httpContext
        )
        {
            _context = context;
            _emailSender = emailSender;
            _httpContext = httpContext;
        }

        public async Task<DtoResult<string>> Handle(InviteTeachersRequest request, CancellationToken cancellationToken)
        {
            var org = await _context.FindAsync<Org>(request.OrgId);

            if (org == null)
            {
                _response.AddError(nameof(request.OrgId), $"Could not find the organization with id of {request.OrgId}.");
                return _response;
            }

            foreach (var email in request.Emails)
            {
                var user = _context.Users.FirstOrDefault(u => u.Email == email);
                if (user == null)
                {
                    org.AddTeacherInvite(email, _context);
                    var callbackUrl = $"{_httpContext.GetBaseUrl()}/invite?email={email}";
                    await _emailSender.SendEmailAsync(email, "Welcome to VocabKitchen",
                        $"You've been invited to VocabKitchen!  Please <a href='{callbackUrl}'>create your account</a>.");
                    continue;
                }

                // at this point go ahead and add them to org
                org.AddTeacher(user, _context);
            }

            _response.AddErrors(org);

            if (_response.HasErrors()) return _response;

            _context.SaveChanges();
            _response.SetValue("Your invitations were sent.");

            return _response;
        }
    }
}
