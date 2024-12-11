using System.Collections.Generic;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using VkCore.Models.TagModel;
using VkCore.Requests.Organization;
using VkCore.SharedKernel;
using VkInfrastructure.Data;

namespace VkInfrastructure.RequestHandlers.Organization
{
    public class AddOrgTagRequestHandler : IRequestHandler<AddOrgTagRequest, DtoResult<IEnumerable<Tag>>>
    {
        private readonly VkDbContext _context;
        private readonly DtoResult<IEnumerable<Tag>> _response;

        public AddOrgTagRequestHandler(VkDbContext context)
        {
            _context = context;
            _response = new DtoResult<IEnumerable<Tag>>();
        }

        public async Task<DtoResult<IEnumerable<Tag>>> Handle(AddOrgTagRequest request, CancellationToken cancellationToken)
        {
            var org = await _context.Organizations.FindAsync(request.OrgId);

            if (org == null)
            {
                _response.AddError(nameof(request.OrgId), $"Could not find the organization with id of {request.OrgId}.");
                return _response;
            }

            org.UpsertTag(request.TagValue, _context);

            _response.AddErrors(org);
            if (_response.HasErrors()) return _response;

            await _context.SaveChangesAsync(cancellationToken);

            _response.SetValue(org.Tags);

            return _response;
        }
    }
}
