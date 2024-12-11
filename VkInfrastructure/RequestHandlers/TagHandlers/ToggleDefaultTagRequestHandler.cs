using System.Collections.Generic;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VkCore.Dtos;
using VkCore.Requests.TagRequest;
using VkCore.SharedKernel;
using VkInfrastructure.Data;

namespace VkInfrastructure.RequestHandlers.TagHandlers
{
    public class ToggleDefaultTagRequestHandler : IRequestHandler<ToggleDefaultTagRequest, DtoResult<IEnumerable<TagDto>>>
    {
        private readonly VkDbContext _context;
        private readonly DtoResult<IEnumerable<TagDto>> _response;

        public ToggleDefaultTagRequestHandler(VkDbContext context)
        {
            _context = context;
            _response = new DtoResult<IEnumerable<TagDto>>();
        }

        public async Task<DtoResult<IEnumerable<TagDto>>> Handle(ToggleDefaultTagRequest request, CancellationToken cancellationToken)
        {
            var tag = await _context.Tags
                .Where(r => r.Id == request.TagId).FirstOrDefaultAsync(cancellationToken);

            if (tag == null)
            {
                _response.AddError(nameof(request.TagId), $"Could not find the tag with id of {request.TagId}.");
                return _response;
            }

            // hold new toggled value state
            var newToggledValue = !tag.IsDefault;

            // clear out all default tags
            foreach (var oldDefaultTag in _context.Tags.Where(t => t.OrgId == tag.OrgId && t.IsDefault == true))
            {
                oldDefaultTag.IsDefault = false;
            }
            
            // set new state
            tag.IsDefault = newToggledValue;

            if (_response.HasErrors()) return _response;
            await _context.SaveChangesAsync(cancellationToken);

            var tags = _context.Tags.Where(t => t.OrgId == tag.OrgId);

            var tagDtos = new List<TagDto>();
            foreach (var orgTag in tags)
            {
                tagDtos.Add(new TagDto(orgTag.Id, orgTag.Value, orgTag.IsDefault));
            }

            _response.SetValue(tagDtos);
            return _response;
        }
    }
}
