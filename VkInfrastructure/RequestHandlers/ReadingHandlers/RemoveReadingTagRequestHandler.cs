using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using VkCore.Dtos;
using VkCore.Requests.ReadingRequest;
using VkCore.SharedKernel;
using VkInfrastructure.Data;

namespace VkInfrastructure.RequestHandlers.ReadingHandlers
{
    public class RemoveReadingTagRequestHandler : IRequestHandler<RemoveReadingTagRequest, DtoResult<IEnumerable<TagDto>>>
    {
        private readonly VkDbContext _context;
        private readonly DtoResult<IEnumerable<TagDto>> _response;

        public RemoveReadingTagRequestHandler(VkDbContext context)
        {
            _context = context;
            _response = new DtoResult<IEnumerable<TagDto>>();
        }

        public async Task<DtoResult<IEnumerable<TagDto>>> Handle(RemoveReadingTagRequest request, CancellationToken cancellationToken)
        {
            var reading = await _context.Readings.Where(x => x.Id == request.ReadingId)
                .Include(x => x.Tags)
                .ThenInclude(x => x.Tag)
                .FirstOrDefaultAsync(cancellationToken);

            if (reading == null)
            {
                _response.AddError(nameof(request.ReadingId), $"Could not find the reading with id of {request.ReadingId}.");
                return _response;
            }

            var tag = await _context.Tags.Where(x => x.Id == request.TagId)
                .FirstOrDefaultAsync(cancellationToken);

            if (tag == null)
            {
                _response.AddError(nameof(request.TagId), $"Could not find the tag with id of {request.TagId}.");
                return _response;
            }

            reading.RemoveTag(tag, _context);

            _response.AddErrors(reading);
            if (_response.HasErrors()) return _response;

            await _context.SaveChangesAsync(cancellationToken);

            _response.SetValue(TagDto.BuildTagDtos(reading.Tags));

            return _response;
        }
    }
}
