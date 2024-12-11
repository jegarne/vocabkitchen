using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using VkCore.Requests.ReadingRequest;
using VkCore.SharedKernel;
using VkInfrastructure.Data;

namespace VkInfrastructure.RequestHandlers.ReadingHandlers
{
    public class DeleteReadingRequestHandler : IRequestHandler<DeleteReadingRequest, DtoResult<string>>
    {
        private readonly VkDbContext _context;
        private readonly DtoResult<string> _response;

        public DeleteReadingRequestHandler(VkDbContext context)
        {
            _context = context;
            _response = new DtoResult<string>();
        }

        public async Task<DtoResult<string>> Handle(DeleteReadingRequest request, CancellationToken cancellationToken)
        {
            var reading = await _context.Readings.Where(x => x.Id == request.ReadingId)
                            .FirstOrDefaultAsync(cancellationToken);

            if (reading == null)
            {
                _response.AddError(nameof(request.ReadingId), $"Could not find the reading with id of {request.ReadingId}.");
                return _response;
            }

            _context.Remove(reading);

            if (_response.HasErrors()) return _response;

            await _context.SaveChangesAsync(cancellationToken);

            _response.SetValue($"{reading.Title} was successfully deleted.");

            return _response;
        }
    }
}
