using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using VkCore.Models.Organization;
using VkCore.Requests.Organization;
using VkCore.SharedKernel;
using VkInfrastructure.Data;

namespace VkInfrastructure.RequestHandlers.Organization
{
    public class GetOrgRequestHandler : IRequestHandler<GetOrgRequest, DtoResult<Org>>
    {
        private readonly VkDbContext _context;
        private readonly DtoResult<Org> _response;

        public GetOrgRequestHandler(VkDbContext context)
        {
            _context = context;
            _response = new DtoResult<Org>();
        }

        public Task<DtoResult<Org>> Handle(GetOrgRequest request, CancellationToken cancellationToken)
        {
            var result = _context.Organizations
                .Include(o => o.Admins)
                .Include(o => o.Teachers)
                .Include(o => o.Students)
                .Include(o => o.Invites)
                .FirstOrDefault();

            if(result == null)
                _response.AddError($"No org was found for id {request.OrgId}");

            _response.SetValue(result);

            return Task.FromResult(_response);
        }
    }
}
