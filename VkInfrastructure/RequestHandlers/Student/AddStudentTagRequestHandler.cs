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
    public class AddStudentTagRequestHandler : IRequestHandler<AddStudentTagRequest, DtoResult<IEnumerable<TagDto>>>
    {
        private readonly VkDbContext _context;
        private readonly DtoResult<IEnumerable<TagDto>> _response;

        public AddStudentTagRequestHandler(VkDbContext context)
        {
            _context = context;
            _response = new DtoResult<IEnumerable<TagDto>>();
        }

        public async Task<DtoResult<IEnumerable<TagDto>>> Handle(AddStudentTagRequest request, CancellationToken cancellationToken)
        {
            var student = await _context.Users.FindAsync(request.StudentId);

            if (student == null)
            {
                _response.AddError(nameof(request.StudentId), $"Could not find the student with id of {request.StudentId}.");
                return _response;
            }

            var tag = await _context.Tags.Where(x => x.Id == request.TagId)
                .FirstOrDefaultAsync(cancellationToken);

            if (tag == null)
            {
                _response.AddError(nameof(request.TagId), $"Could not find the tag with id of {request.TagId}.");
                return _response;
            }

            tag.AddUser(request.StudentId, _context);

            if (_response.HasErrors()) return _response;

            await _context.SaveChangesAsync(cancellationToken);

            student = await _context.Users
                .Include(u => u.UserTags)
                .ThenInclude(t => t.Tag)
                .FirstOrDefaultAsync(s => s.Id == request.StudentId, cancellationToken);

            _response.SetValue(TagDto.BuildTagDtos(student.UserTags));

            return _response;
        }
    }
}
