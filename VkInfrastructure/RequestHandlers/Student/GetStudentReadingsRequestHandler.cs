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
    public class GetStudentReadingsRequestHandler : IRequestHandler<GetStudentReadingsRequest, DtoResult<IEnumerable<ReadingDto>>>
    {
        private readonly VkDbContext _context;
        private readonly DtoResult<IEnumerable<ReadingDto>> _response = new DtoResult<IEnumerable<ReadingDto>>();

        public GetStudentReadingsRequestHandler(VkDbContext context)
        {
            _context = context;
        }

        public async Task<DtoResult<IEnumerable<ReadingDto>>> Handle(GetStudentReadingsRequest request, CancellationToken cancellationToken)
        {
            var tagIds = await _context.UserTags
                .Where(u => u.UserId == request.StudentId)
                .Select(t => t.TagId).ToListAsync(cancellationToken);
            
            if (tagIds == null)
            {
                _response.SetValue(new List<ReadingDto>());
                return _response;
            }

            var readings =  await _context.Readings
                .Include(r => r.ContentItems)
                .Where(r => r.Tags.Any(t => tagIds.Contains(t.TagId)))
                .ToListAsync(cancellationToken);

            var studentWords = _context.StudentWords.Where(w => w.VkUserId == request.StudentId).ToList();

            var result = ReadingDto.BuildReadingDtos(readings, studentWords);

            if (_response.HasErrors()) return _response;
            _response.SetValue(result);

            return _response;
        }
    }
}

