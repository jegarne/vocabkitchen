using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using VkCore.Dtos;
using VkCore.Models;
using VkCore.Requests.ReadingRequest;
using VkCore.Services;
using VkCore.SharedKernel;
using VkInfrastructure.Data;

namespace VkInfrastructure.RequestHandlers.ReadingHandlers
{
    public class UpdateReadingRequestHandler : IRequestHandler<UpdateReadingRequest, DtoResult<ReadingDto>>
    {
        private readonly VkDbContext _context;
        private readonly DtoResult<ReadingDto> _response;

        public UpdateReadingRequestHandler(VkDbContext context)
        {
            _context = context;
            _response = new DtoResult<ReadingDto>();
        }

        public async Task<DtoResult<ReadingDto>> Handle(UpdateReadingRequest request, CancellationToken cancellationToken)
        {
            var reading = await _context.Readings.Where(x => x.Id == request.ReadingId)
                .Include(o => o.ContentItems)
                .FirstOrDefaultAsync(cancellationToken);

            if (reading == null)
            {
                _response.AddError(nameof(request.ReadingId), $"Could not find the reading with id of {request.ReadingId}.");
                return _response;
            }

            reading.Title = request.Title;

            foreach (var edit in request.Edits)
            {
                if (edit.Type.ToLower() == "insert"  || edit.Type.ToLower() == "paste")
                {
                    reading.InsertContent(edit.Start, edit.Value);
                    continue;
                }

                if (edit.Type.ToLower() == "delete")
                {
                    reading.Delete(edit.Start, edit.End);
                }
            }

            reading.CleanupContentItems();

            // remove any annotation contexts from words no longer referenced in the reading
            var currentAnotCtxtIds = reading.ContentItems
                .Where(y => !string.IsNullOrEmpty(y.AnnotationContextId)).Select(x => x.AnnotationContextId);
            var savedAnotCtxtIds = _context.AnnotationContexts
                .Where(x => x.ReadingId == reading.Id).Select(y => y.Id).ToList();
            var idsToDelete = savedAnotCtxtIds.Where(i => !currentAnotCtxtIds.Contains(i));

            foreach (var id in idsToDelete)
            {
                var toDelete = _context.AnnotationContexts
                    .Where(x => idsToDelete.Contains(x.Id)).ToList();
                _context.RemoveRange(toDelete);
            }

            _response.AddErrors(reading);
            if (_response.HasErrors()) return _response;

            await _context.SaveChangesAsync(cancellationToken);
            
            // update example sentences pulled from this reading
            var sentenceService = new ExampleSentenceService(reading.Text);
            foreach (var ci in reading.ContentItems.Where(x => !string.IsNullOrEmpty(x.AnnotationContextId)))
            {
                var annotCtxt = await _context.AnnotationContexts
                    .FirstOrDefaultAsync(x => x.Id == ci.AnnotationContextId, cancellationToken);
                if (annotCtxt != null)
                {
                    var sentence = sentenceService.GetSentenceAtIndex(ci.FirstIndex);
                    annotCtxt.Value = sentence;
                }
            }

            await _context.SaveChangesAsync(cancellationToken);

            _response.SetValue(new ReadingDto(reading));

            return _response;
        }
    }
}
