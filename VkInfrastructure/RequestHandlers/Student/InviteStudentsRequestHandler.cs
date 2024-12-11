using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using VkCore.Interfaces;
using VkCore.Models.Organization;
using VkCore.Requests.Student;
using VkCore.SharedKernel;
using VkInfrastructure.Data;
using VkWeb.Extensions;

namespace VkInfrastructure.RequestHandlers.Student
{
    public class InviteStudentsRequestHandler : IRequestHandler<InviteStudentsRequest, DtoResult<string>>
    {
        private readonly VkDbContext _context;
        private readonly IEmailSender _emailSender;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IVkConfig _config;
        private readonly DtoResult<string> _response = new DtoResult<string>();

        public InviteStudentsRequestHandler(
            VkDbContext context,
            IEmailSender emailSender,
            IHttpContextAccessor httpContext,
            IVkConfig config
        )
        {
            _context = context;
            _emailSender = emailSender;
            _httpContext = httpContext;
            _config = config;
        }


        public async Task<DtoResult<string>> Handle(InviteStudentsRequest request, CancellationToken cancellationToken)
        {
            var org = await _context.FindAsync<Org>(request.OrgId);

            if (org == null)
            {
                _response.AddError(nameof(request.OrgId), $"Could not find the organization with id of {request.OrgId}.");
                return _response;
            }

            if(!request.Emails.Any())
            {
                _response.AddError(nameof(request.OrgId), $"Please add emails so we can invite students.");
                return _response;
            }

            if (!org.CanAddStudents(request.Emails.Count(), _config, _context))
            {
                _response.AddErrors(org);
                return _response;
            }

            foreach (var email in request.Emails)
            {
                var user = _context.Users.FirstOrDefault(u => u.Email == email);
                if (user == null)
                {
                    org.AddStudentInvite(email, _context);
                    var callbackUrl = $"{_httpContext.GetBaseUrl()}/invite?email={email}";
                    await _emailSender.SendEmailAsync(email, "Welcome to VocabKitchen",
                        $"You've been invited to VocabKitchen!  Please <a href='{callbackUrl}'>create your account</a>.");
                    continue;
                }

                // at this point go ahead and add them to org
                org.AddStudent(user, _context);
            }

            _response.AddErrors(org);

            if (_response.HasErrors()) return _response;

            await _context.SaveChangesAsync(cancellationToken);
            _response.SetValue("Your invitations were sent.");

            return _response;
        }
    }
}
