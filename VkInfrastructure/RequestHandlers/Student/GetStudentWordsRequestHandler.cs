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
    public class GetStudentWordsRequestHandler : IRequestHandler<GetStudentWordsRequest, DtoResult<IEnumerable<StudentWordDto>>>
    {
        private readonly VkDbContext _context;
        private readonly DtoResult<IEnumerable<StudentWordDto>> _response = new DtoResult<IEnumerable<StudentWordDto>>();

        public GetStudentWordsRequestHandler(VkDbContext context)
        {
            _context = context;
        }

        public async Task<DtoResult<IEnumerable<StudentWordDto>>> Handle(GetStudentWordsRequest request, CancellationToken cancellationToken)
        {
            var dtos = await _context.StudentWords
                .Include(w => w.Attempts)
                .Include(w => w.WordEntry)
                .Include(w => w.Annotation)
                .ThenInclude(a => a.AnnotationContexts)
                .Where(u => u.VkUserId == request.StudentId)
                .Select(t => new StudentWordDto(t))
                .ToListAsync(cancellationToken);

            _response.SetValue(dtos);

            return _response;
        }
    }
}

