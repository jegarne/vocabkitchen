using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VkCore.Dtos;
using VkCore.Requests.Organization;
using VkCore.SharedKernel;
using VkInfrastructure.Data;
using VkInfrastructure.Services;

namespace VkInfrastructure.RequestHandlers.Organization
{
    public class GetStudentProgressRequestHandler : IRequestHandler<GetStudentProgressRequest, DtoResult<StudentProgressDto>>
    {
        private readonly VkDbContext _context;
        private readonly DtoResult<StudentProgressDto> _response = new DtoResult<StudentProgressDto>();

        public GetStudentProgressRequestHandler(VkDbContext context)
        {
            _context = context;
        }

        public async Task<DtoResult<StudentProgressDto>> Handle(GetStudentProgressRequest request, CancellationToken cancellationToken)
        {
            var student = await _context.Users
                .FirstOrDefaultAsync(w => w.Id == request.StudentId, cancellationToken);

            if (student == null)
            {
                _response.AddError(nameof(request.StudentId), $"Could not find the student with id of {request.StudentId}.");
                return _response;
            }

            var result = new StudentProgressDto(student);

            var studentWords = await _context.StudentWords
                .Include(w => w.WordEntry)
                .Include(w => w.Attempts)
                .Where(w => w.VkUserId == request.StudentId)
                .ToListAsync(cancellationToken);

            var studentDataService = new StudentDataService(_context, request.StudentId);
            result.SetProgressCounts(studentDataService);

            foreach (var w in studentWords)
            {
                result.AddWord(w);
            }

            if (_response.HasErrors()) return _response;
            _response.SetValue(result);

            return _response;
        }
    }
}
