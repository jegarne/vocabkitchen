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
    public class DeleteTagRequestHandler : IRequestHandler<DeleteTagRequest, DtoResult<TagDto>>
    {
        private readonly VkDbContext _context;
        private readonly DtoResult<TagDto> _response;

        public DeleteTagRequestHandler(VkDbContext context)
        {
            _context = context;
            _response = new DtoResult<TagDto>();
        }

        public async Task<DtoResult<TagDto>> Handle(DeleteTagRequest request, CancellationToken cancellationToken)
        {
            var tag = await _context.Tags
                .Include(x => x.Readings)
                .Include(x => x.Users)
                .Where(r => r.Id == request.TagId).FirstOrDefaultAsync(cancellationToken);

            if (tag == null)
            {
                _response.AddError(nameof(request.TagId), $"Could not find the tag with id of {request.TagId}.");
                return _response;
            }

            if (tag.Users.Any())
            {
                _response.AddError(nameof(request.TagId), $"Please remove this tag from any users before you delete it.");
            }

            if (tag.Readings.Any())
            {
                _response.AddError(nameof(request.TagId), $"Please remove this tag from any readings before you delete it.");
            }

            _response.SetValue(new TagDto(tag.Id, tag.Value, tag.IsDefault));

            if (_response.HasErrors()) return _response;

            _context.Remove(tag);
            await _context.SaveChangesAsync(cancellationToken);

            return _response;
        }
    }
}
