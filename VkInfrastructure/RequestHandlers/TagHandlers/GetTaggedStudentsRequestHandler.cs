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
    public class GetTaggedStudentsRequestHandler : IRequestHandler<GetTaggedStudentsRequest, DtoResult<IEnumerable<StudentDto>>>
    {
        private readonly VkDbContext _context;
        private readonly DtoResult<IEnumerable<StudentDto>> _response;

        public GetTaggedStudentsRequestHandler(VkDbContext context)
        {
            _context = context;
            _response = new DtoResult<IEnumerable<StudentDto>>();
        }

        public Task<DtoResult<IEnumerable<StudentDto>>> Handle(GetTaggedStudentsRequest request, CancellationToken cancellationToken)
        {
            var students = _context.Users
                .Include(x => x.UserTags)
                .ThenInclude(x => x.Tag)
                .Where(r => r.UserTags.Any(t => t.Tag.Id == request.TagId)).ToList();

            var dto = StudentDto.BuildStudentDtos(students);
            _response.SetValue(dto);

            return Task.FromResult(_response);
        }
    }
}
