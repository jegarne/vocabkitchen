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
    public class GetUserOrgRequestHandler : IRequestHandler<GetUserOrgRequest, DtoResult<OrgDto>>
    {
        private readonly DtoResult<OrgDto> _response = new DtoResult<OrgDto>();
        private readonly VkDbContext _context;

        public GetUserOrgRequestHandler(
            VkDbContext context
        )
        {
            _context = context;
        }

        public async Task<DtoResult<OrgDto>> Handle(GetUserOrgRequest request, CancellationToken cancellationToken)
        {
            var org = await _context.Organizations
                .Include(o => o.Tags)
                .Include(o => o.Students)
                .ThenInclude(x => x.StudentUser)
                .Include(o => o.Readings)
                .ThenInclude(x => x.Reading)
                .ThenInclude(o => o.ContentItems)
                .FirstOrDefaultAsync(x => x.Id == request.OrgId, cancellationToken);

            if (org == null)
            {
                _response.AddError(nameof(request.OrgId), $"Could not find the org with id of {request.OrgId}.");
                return _response;
            }

            _response.AddErrors(org);
            if (_response.HasErrors()) return _response;

            _response.SetValue(new OrgDto(org));

            return _response;
        }
    }
}
