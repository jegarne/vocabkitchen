using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VkCore.Builders;
using VkCore.Dtos;
using VkCore.Events.Word;
using VkCore.Models.Word;
using VkCore.Requests.ReadingRequest;
using VkCore.SharedKernel;
using VkInfrastructure.Data;

namespace VkInfrastructure.RequestHandlers.ReadingHandlers
{
    /// <summary>
    /// Associates a new definition
    /// </summary>
    public class AddDefinitionRequestHandler : IRequestHandler<AddDefinitionRequest, DtoResult<ReadingDto>>
    {
        private readonly VkDbContext _context;
        private readonly DtoResult<ReadingDto> _response;
        private readonly IMediator _mediator;

        public AddDefinitionRequestHandler(VkDbContext context, IMediator mediator)
        {
            _context = context;
            _response = new DtoResult<ReadingDto>();
            _mediator = mediator;
        }

        public async Task<DtoResult<ReadingDto>> Handle(AddDefinitionRequest request, CancellationToken cancellationToken)
        {
            if (request.Word.Length > 50)
            {
                _response.AddError("DefinitionError", $"You cannot define a word or phrase longer than 50 characters.");
                return _response;
            }

            var reading = await _context.Readings.Where(x => x.Id == request.ReadingId)
                .Include(o => o.ContentItems)
                .FirstOrDefaultAsync(cancellationToken);

            if (reading == null)
            {
                _response.AddError(nameof(request.ReadingId), $"Could not find the reading with id of {request.ReadingId}.");
                return _response;
            }

            var wordEntry = await _context.Words
                .Include(x => x.Annotations)
                .Where(w => w.Word == request.Word)
                .FirstOrDefaultAsync(cancellationToken);

            if (wordEntry == null)
            {
                wordEntry = new WordEntry(request.Word);
                _context.Words.Add(wordEntry);
            }

            Annotation existingAnnotation;

            if (!string.IsNullOrEmpty(request.AnnotationId))
            {
                // if we have an id, update the value 
                // (this is the edge case where we've selected 
                // an editable, user-created annotation)
                existingAnnotation = wordEntry.Annotations.FirstOrDefault(a => a.Id == request.AnnotationId);
                if (existingAnnotation != null)
                    existingAnnotation.Value = request.Definition;
            }
            else
            {
                existingAnnotation = wordEntry.Annotations.FirstOrDefault(a => a.Value == request.Definition);
            }

            var wordContext = reading.GetWordContext(request);
            if (existingAnnotation == null)
            {
                // this is a new definition for this word, so build new annotation
                var builder = new DefinitionBuilder(wordEntry.Id);
                builder.SetContent(request.Definition, request.PartOfSpeech);
                builder.AddExampleSentence(wordContext, request.ReadingId);
                builder.SetSource(request.Source, request.UserId);
                wordEntry.AddAnnotation(builder.GetAnnotation(), _context);
            }
            else
            {
                // this word already has this definition, but let's make sure it has this context
                existingAnnotation.AddContext(wordContext, request.ReadingId, _context);
            }

            var annotation = wordEntry.Annotations.First(a => a.Value == request.Definition);
            var annotationContext = annotation.AnnotationContexts.First(a => a.Value == wordContext);
            var newContentItem = reading.InsertDefinition(request, wordEntry.Id, annotation.Id, annotationContext.Id, _context);
            // set defined content item id on annotation context so we can 
            // target correct context if definition is later removed
            annotationContext.ContentItemId = newContentItem.Id;

            _response.AddErrors(reading);
            if (_response.HasErrors()) return _response;

            await _context.SaveChangesAsync(cancellationToken);

            _response.SetValue(await ReadingDto.BuildReadingDto(reading, _context));

            return _response;
        }
    }
}
