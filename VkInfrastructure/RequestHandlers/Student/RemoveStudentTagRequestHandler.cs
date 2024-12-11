using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VkCore.Dtos;
using VkCore.Requests.Student;
using VkCore.SharedKernel;
using VkInfrastructure.Data;

namespace VkInfrastructure.RequestHandlers.Student
{
    public class RemoveStudentTagRequestHandler : IRequestHandler<RemoveStudentTagRequest, DtoResult<IEnumerable<TagDto>>>
    {
        private readonly VkDbContext _context;
        private readonly DtoResult<IEnumerable<TagDto>> _response;

        public RemoveStudentTagRequestHandler(VkDbContext context)
        {
            _context = context;
            _response = new DtoResult<IEnumerable<TagDto>>();
        }

        public async Task<DtoResult<IEnumerable<TagDto>>> Handle(RemoveStudentTagRequest request, CancellationToken cancellationToken)
        {
            var student = await _context.Users
                .Include(u => u.UserTags)
                .ThenInclude(u => u.Tag)
                .FirstOrDefaultAsync(x => x.Id == request.StudentId, cancellationToken);

            if (student == null)
            {
                _response.AddError(nameof(request.StudentId), $"Could not find the student with id of {request.StudentId}.");
                return _response;
            }

            var tag = await _context.Tags
                .Include(t => t.Users)
                .FirstOrDefaultAsync(x => x.Id == request.TagId, cancellationToken);

            if (tag == null)
            {
                _response.AddError(nameof(request.TagId), $"Could not find the tag with id of {request.TagId}.");
                return _response;
            }

            tag.RemoveUser(request.StudentId, _context);

            if (_response.HasErrors()) return _response;

            await _context.SaveChangesAsync(cancellationToken);

            _response.SetValue(TagDto.BuildTagDtos(student.UserTags));

            return _response;
        }
    }
}
