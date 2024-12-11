using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using VkCore.Dtos;
using VkCore.Requests.TagRequest;
using VkCore.SharedKernel;
using VkInfrastructure.Data;

namespace VkInfrastructure.RequestHandlers.TagHandlers
{
    public class MergeStudentTagsRequestHandler : IRequestHandler<MergeStudentTagsRequest, DtoResult<IEnumerable<StudentDto>>>
    {
        private readonly VkDbContext _context;
        private readonly DtoResult<IEnumerable<StudentDto>> _response;

        public MergeStudentTagsRequestHandler(VkDbContext context)
        {
            _context = context;
            _response = new DtoResult<IEnumerable<StudentDto>>();
        }

        public async Task<DtoResult<IEnumerable<StudentDto>>> Handle(MergeStudentTagsRequest request, CancellationToken cancellationToken)
        {
            var tag = await _context.Tags
                .Include(x => x.Users)
                .FirstOrDefaultAsync(t => t.Id == request.TagId, cancellationToken);
            
            if (tag == null)
            {
                _response.AddError(nameof(request.TagId), $"Could not find the tag with id of {request.TagId}.");
                return _response;
            }

            tag.MergeUsers(request.StudentIds, _context);

            if (_response.HasErrors()) return _response;

            _context.SaveChanges();

            var students = _context.Users
                .Include(u => u.UserTags)
                .ThenInclude(t => t.Tag)
                .Where(r => r.UserTags.Any(t => t.Tag.Id == request.TagId)).ToList();

            _response.SetValue(StudentDto.BuildStudentDtos(students));

            return _response;
        }
    }
}
