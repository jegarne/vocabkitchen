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
    public class GetTaggedReadingsRequestHandler : IRequestHandler<GetTaggedReadingsRequest, DtoResult<IEnumerable<ReadingDto>>>
    {
        private readonly VkDbContext _context;
        private readonly DtoResult<IEnumerable<ReadingDto>> _response;

        public GetTaggedReadingsRequestHandler(VkDbContext context)
        {
            _context = context;
            _response = new DtoResult<IEnumerable<ReadingDto>>();
        }

        public Task<DtoResult<IEnumerable<ReadingDto>>> Handle(GetTaggedReadingsRequest request, CancellationToken cancellationToken)
        {
            var readings = _context.Readings
                .Include(x => x.Tags)
                .ThenInclude(x => x.Tag)
                .Where(r => r.Tags.Any(t => t.Tag.Id == request.TagId)).ToList();

            var dto = ReadingDto.BuildReadingDtos(readings);
            _response.SetValue(dto);

            return Task.FromResult(_response);
        }
    }
}
