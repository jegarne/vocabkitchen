using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VkCore.Dtos;
using VkCore.Requests.Student;
using VkCore.SharedKernel;
using VkInfrastructure.Data;
using VkInfrastructure.Services;

namespace VkInfrastructure.RequestHandlers.Student
{
    public class GetStudentDetailsRequestHandler : IRequestHandler<GetStudentDetailsRequest, DtoResult<StudentDto>>
    {
        private readonly VkDbContext _context;
        private readonly DtoResult<StudentDto> _response = new DtoResult<StudentDto>();

        public GetStudentDetailsRequestHandler(VkDbContext context)
        {
            _context = context;
        }

        public async Task<DtoResult<StudentDto>> Handle(GetStudentDetailsRequest request, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .Include(u => u.UserTags)
                .ThenInclude(u => u.Tag)
                .FirstOrDefaultAsync(u => u.Id == request.StudentId, cancellationToken);

            if (user == null)
            {
                _response.AddError(nameof(request.StudentId), $"Could not find the user with id of {request.StudentId}.");
                return _response;
            }

            var dto = new StudentDto(user, TagDto.BuildTagDtos(user.UserTags));
            var studentDataService = new StudentDataService(_context, user.Id);
            dto.SetProgressCounts(studentDataService);

            if (_response.HasErrors()) return _response;
            _response.SetValue(dto);

            return _response;
        }
    }
}
