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
    public class GetReadingRequestHandler : IRequestHandler<GetReadingRequest, DtoResult<ReadingDto>>
    {
        private readonly VkDbContext _context;
        private readonly DtoResult<ReadingDto> _response;

        public GetReadingRequestHandler(VkDbContext context)
        {
            _context = context;
            _response = new DtoResult<ReadingDto>();
        }

        public async Task<DtoResult<ReadingDto>> Handle(GetReadingRequest request, CancellationToken cancellationToken)
        {
            var reading = await _context.Readings.Where(x => x.Id == request.ReadingId)
                .Include(o => o.ContentItems)
                .Include(o => o.Tags)
                .ThenInclude(x => x.Tag)
                .FirstOrDefaultAsync(cancellationToken);

            if (reading == null)
            {
                _response.AddError(nameof(request.ReadingId), $"Could not find the reading with id of {request.ReadingId}.");
                return _response;
            }
            
            var studentWords = _context.StudentWords.Where(w => w.VkUserId == request.StudentId).ToList();

            if (_response.HasErrors()) return _response;

            _response.SetValue(await ReadingDto.BuildReadingDto(reading, _context, studentWords));

            return _response;
        }
    }
}
