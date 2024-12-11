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
    public class RemoveDefinitionRequestHandler : IRequestHandler<RemoveDefinitionRequest, DtoResult<ReadingDto>>
    {
        private readonly VkDbContext _context;
        private readonly DtoResult<ReadingDto> _response;

        public RemoveDefinitionRequestHandler(VkDbContext context)
        {
            _context = context;
            _response = new DtoResult<ReadingDto>();
        }

        public async Task<DtoResult<ReadingDto>> Handle(RemoveDefinitionRequest request, CancellationToken cancellationToken)
        {
            var reading = await _context.Readings.Where(x => x.Id == request.ReadingId)
                .Include(o => o.ContentItems)
                .FirstOrDefaultAsync(cancellationToken);

            if (reading == null)
            {
                _response.AddError(nameof(request.ReadingId), $"Could not find the reading with id of {request.ReadingId}.");
                return _response;
            }

            var contentItemToUndefine = reading.ContentItems.FirstOrDefault(a => a.Id == request.ContentItemId);
            if (contentItemToUndefine == null)
            {
                _response.AddError(nameof(request.ContentItemId), $"Attempted to remove a definition from content item {request.ReadingId} but it was not defined.");
                return _response;
            }

            var wordId = contentItemToUndefine.WordId;
            var annotationId = contentItemToUndefine.AnnotationId;

            reading.RemoveDefinition(request.ContentItemId);

             // remove context created when this word was defined
            var annotContextToRemove = await _context.AnnotationContexts
                .FirstOrDefaultAsync(c => c.ReadingId == request.ReadingId && c.ContentItemId == request.ContentItemId, cancellationToken);
            if (annotContextToRemove != null) _context.Remove(annotContextToRemove);

                       // if definition was only used on one word, remove all additional references to it

            var wordEntry = await _context.Words
                .Include(w => w.Annotations)
                .ThenInclude(w => w.AnnotationContexts)
                .Include(w => w.Students)
                .ThenInclude(y => y.Attempts)
                .FirstOrDefaultAsync(x => x.Id == wordId);

            // remove related student attempts and students
            var wordAttempts = wordEntry?.Students.Where(x => x.AnnotationId == annotationId).ToList();
            if (wordAttempts != null)
            {
                foreach (var attempt in wordAttempts)
                {
                    _context.RemoveRange(attempt.Attempts);
                    _context.Remove(attempt);
                }
            }

            // remove annotation from word
            var annotation = wordEntry?.Annotations.FirstOrDefault(x => x.Id == annotationId);
            if (annotation?.AnnotationContexts != null)
            {
                _context.RemoveRange(annotation.AnnotationContexts);
            }
            if (annotation != null)
            {
                _context.Remove(annotation);
            }

            // if word has no remaining annotations, delete word entry
            if (wordEntry != null && (wordEntry?.Annotations == null || wordEntry?.Annotations?.Count(c => c.Id != annotation?.Id) == 0))
            {
                _context.Remove(wordEntry);
            }

               _response.AddErrors(reading);
            if (_response.HasErrors()) return _response;

            await _context.SaveChangesAsync(cancellationToken);



            _response.SetValue(await ReadingDto.BuildReadingDto(reading, _context));

            return _response;
        }
    }
}
