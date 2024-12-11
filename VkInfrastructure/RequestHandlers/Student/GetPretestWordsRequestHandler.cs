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
    public class GetPretestWordsRequestHandler : IRequestHandler<GetPretestWordsRequest, DtoResult<IEnumerable<StudentWordDto>>>
    {
        private readonly VkDbContext _context;
        private readonly DtoResult<IEnumerable<StudentWordDto>> _response = new DtoResult<IEnumerable<StudentWordDto>>();

        public GetPretestWordsRequestHandler(VkDbContext context)
        {
            _context = context;
        }

        public async Task<DtoResult<IEnumerable<StudentWordDto>>> Handle(GetPretestWordsRequest request, CancellationToken cancellationToken)
        {
            var studentWordIds = await _context.StudentWords
                .Where(w => w.VkUserId == request.UserId)
                .Select(w => w.WordEntryId).ToListAsync() ?? new List<string>();

            var dtos = await _context.ContentItems
                .Include(w => w.Word)
                .Include(w => w.Annotation)
                .ThenInclude(a => a.AnnotationContexts)
                .Where(c => c.ReadingId == request.ReadingId && c.WordId != null
                        && !studentWordIds.Contains(c.WordId))
                .Select(c => new StudentWordDto(
                    c.WordId, c.AnnotationId, c.Word.Word, c.Annotation.Value,
                    c.Annotation.AnnotationContexts.Select(a => a.Value)))
                    .ToListAsync();

            _response.SetValue(dtos);

            return _response;
        }
    }
}

