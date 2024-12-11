using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using VkCore.Dtos;
using VkCore.Requests.TagRequest;
using VkCore.SharedKernel;
using VkInfrastructure.Data;

namespace VkInfrastructure.RequestHandlers.TagHandlers
{
    public class MergeReadingTagsRequestHandler : IRequestHandler<MergeReadingTagsRequest, DtoResult<IEnumerable<ReadingDto>>>
    {
        private readonly VkDbContext _context;
        private readonly DtoResult<IEnumerable<ReadingDto>> _response;

        public MergeReadingTagsRequestHandler(VkDbContext context)
        {
            _context = context;
            _response = new DtoResult<IEnumerable<ReadingDto>>();
        }

        public async Task<DtoResult<IEnumerable<ReadingDto>>> Handle(MergeReadingTagsRequest request, CancellationToken cancellationToken)
        {
            var tag = await _context.Tags
                .Include(x => x.Readings)
                .ThenInclude(x => x.Reading)
                .FirstOrDefaultAsync(t => t.Id == request.TagId, cancellationToken);
            
            if (tag == null)
            {
                _response.AddError(nameof(request.TagId), $"Could not find the tag with id of {request.TagId}.");
                return _response;
            }
            tag.MergeReadings(request.ReadingIds, _context);

            _context.SaveChanges();

            var readings = _context.Readings
                .Include(x => x.Tags)
                .ThenInclude(x => x.Tag)
                .Where(r => r.Tags.Any(t => t.Tag.Id == request.TagId)).ToList();

            var dto = ReadingDto.BuildReadingDtos(readings);
            _response.SetValue(dto);

            return _response;
        }
    }
}
