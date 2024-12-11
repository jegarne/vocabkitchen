using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VkCore.Dtos;
using VkCore.Requests.Organization;
using VkCore.SharedKernel;
using VkInfrastructure.Data;

namespace VkInfrastructure.RequestHandlers.Organization
{
    public class GetOrgDetailRequestHandler : IRequestHandler<GetOrgDetailRequest, DtoResult<OrgDetailDto>>
    {
        private readonly DtoResult<OrgDetailDto> _response = new DtoResult<OrgDetailDto>();
        private readonly VkDbContext _context;

        public GetOrgDetailRequestHandler(
            VkDbContext context
        )
        {
            _context = context;
        }

        public async Task<DtoResult<OrgDetailDto>> Handle(GetOrgDetailRequest request, CancellationToken cancellationToken)
        {
            var org = await _context.Organizations
                .Include(o => o.Students)
                .ThenInclude(x => x.StudentUser)
                .Include(o => o.Teachers)
                .ThenInclude(x => x.TeacherUser)
                .Include(o => o.Admins)
                .ThenInclude(x => x.AdminUser)
                .FirstOrDefaultAsync(x => x.Id == request.OrgId, cancellationToken);

            if (org == null)
            {
                _response.AddError(nameof(request.OrgId), $"Could not find the org with id of {request.OrgId}.");
                return _response;
            }

            _response.AddErrors(org);
            if (_response.HasErrors()) return _response;

            _response.SetValue(new OrgDetailDto(org));

            return _response;
        }
    }
}
