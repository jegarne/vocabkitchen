using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VkCore.Constants;
using VkCore.Models;
using VkCore.Models.Organization;
using VkCore.Requests.Organization;
using VkCore.SharedKernel;
using VkInfrastructure.Data;

namespace VkInfrastructure.RequestHandlers.Organization
{
    public class AcceptInviteRequestHandler : IRequestHandler<AcceptInviteRequest, DtoResult<Org>>
    {
        private readonly VkDbContext _context;
        private readonly DtoResult<Org> _response;

        public AcceptInviteRequestHandler(VkDbContext context)
        {
            _context = context;
            _response = new DtoResult<Org>();
        }

        public async Task<DtoResult<Org>> Handle(AcceptInviteRequest request, CancellationToken cancellationToken)
        {
            var org = _context.Organizations.Find(request.OrgId);

            if (org == null)
            {
                _response.AddError(nameof(request.OrgId), $"Could not find the organization with id of {request.OrgId}.");
                return _response;
            }

            var user = _context.Find<VkUser>(request.VkUserId);
            if (user == null)
            {
                _response.AddError(nameof(request.VkUserId), $"Could not find the user with id of {request.VkUserId}.");
                return _response;
            }

            switch (request.UserType.ToLower())
            {
                case UserTypes.Teacher:
                    org.AcceptTeacherInvite(user, _context);
                    break;
                case UserTypes.Student:
                    org.AcceptStudentInvite(user, _context);
                    break;
            }

            _response.AddErrors(org);
            if (_response.HasErrors()) return _response;

            await _context.SaveChangesAsync(cancellationToken);
            _response.SetValue(org);

            return _response;
        }
    }
}
