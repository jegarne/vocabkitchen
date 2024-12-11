using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VkCore.Models.Organization;
using VkCore.Requests.Organization;
using VkCore.SharedKernel;
using VkInfrastructure.Data;

namespace VkInfrastructure.RequestHandlers.Organization
{
    public class RemoveOrgInviteRequestHandler : IRequestHandler<RemoveOrgInviteRequest, DtoResult<Org>>
    {
        private readonly VkDbContext _context;
        private readonly DtoResult<Org> _response;

        public RemoveOrgInviteRequestHandler(VkDbContext context)
        {
            _context = context;
            _response = new DtoResult<Org>();
        }

        public Task<DtoResult<Org>> Handle(RemoveOrgInviteRequest request, CancellationToken cancellationToken)
        {
            var org = _context.Organizations.Find(request.OrgId);

            if (org == null)
            {
                _response.AddError(nameof(request.OrgId), $"Could not find the organization with id of {request.OrgId}.");
                return Task.FromResult(_response);
            }

            org.RemoveInvite(request.Email, _context);

            _response.AddErrors(org);
            if (_response.HasErrors()) Task.FromResult(_response);

            _context.SaveChanges();
            _response.SetValue(org);

            return Task.FromResult(_response);
        }
    }
}
