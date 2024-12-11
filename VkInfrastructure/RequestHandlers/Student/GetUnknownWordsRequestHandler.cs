using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VkCore.Dtos;
using VkCore.Requests.Student;
using VkCore.SharedKernel;
using VkInfrastructure.Data;

namespace VkInfrastructure.RequestHandlers.Student
{
    public class GetUnknownWordsRequestHandler : IRequestHandler<GetUnkownWordsRequest, DtoResult<IEnumerable<StudentWordDto>>>
    {
        private readonly VkDbContext _context;
        private readonly DtoResult<IEnumerable<StudentWordDto>> _response = new DtoResult<IEnumerable<StudentWordDto>>();

        public GetUnknownWordsRequestHandler(VkDbContext context)
        {
            _context = context;
        }

        public async Task<DtoResult<IEnumerable<StudentWordDto>>> Handle(GetUnkownWordsRequest request, CancellationToken cancellationToken)
        {
            var dtos = new List<StudentWordDto>();

            if (string.IsNullOrWhiteSpace(request.ReadingId))
            {
                // if we don't have a reading id
                // return all unknown words for a user
                dtos = await _context.StudentWords
                    .Include(w => w.WordEntry)
                    .Include(w => w.Annotation)
                    .ThenInclude(a => a.AnnotationContexts)
                    .Where(w => w.VkUserId == request.UserId && w.IsKnown == false)
                    .Select(t => new StudentWordDto(t))
                    .ToListAsync(cancellationToken);

                // if we don't have any unknown words send up
                // some known ones to review
                if (!dtos.Any())
                {
                    dtos = await _context.StudentWords
                            .Include(w => w.WordEntry)
                            .Include(w => w.Annotation)
                            .ThenInclude(a => a.AnnotationContexts)
                            .Where(w => w.VkUserId == request.UserId && w.IsKnown == true)
                            .OrderBy(x => x.IsKnownDate)
                            .Take(7)
                            .Select(t => new StudentWordDto(t))
                            .ToListAsync(cancellationToken);
                }
            }
            else
            {
                var knownWordIds = await _context.StudentWords
                    .Where(w => w.VkUserId == request.UserId && w.IsKnown == true)
                    .Select(w => w.WordEntryId).ToListAsync(cancellationToken);

                var unknownWordIds = await _context.StudentWords
                    .Where(w => w.VkUserId == request.UserId && w.IsKnown == false)
                    .Select(w => w.WordEntryId).ToListAsync(cancellationToken);

                // return all word ids from the reading
                dtos = await _context.ContentItems
                    .Include(w => w.Word)
                    .Include(w => w.Annotation)
                    .ThenInclude(a => a.AnnotationContexts)
                    .Where(c => c.ReadingId == request.ReadingId && c.WordId != null
                            && !knownWordIds.Contains(c.WordId)
                            && unknownWordIds.Contains(c.WordId))
                    .Select(c => new StudentWordDto(
                        c.WordId, c.AnnotationId, c.Word.Word, c.Annotation.Value,
                        c.Annotation.AnnotationContexts.Select(a => a.Value)))
                        .ToListAsync();
            }

            _response.SetValue(dtos);

            return _response;
        }
    }
}

