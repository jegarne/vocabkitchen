using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VkCore.Dtos;
using VkCore.Requests.Organization;
using VkCore.SharedKernel;
using VkInfrastructure.Data;
using VkInfrastructure.Services;

namespace VkInfrastructure.RequestHandlers.Organization
{
    public class GetStudentsProgressRequestHandler : IRequestHandler<GetStudentsProgressRequest, DtoResult<IEnumerable<StudentDto>>>
    {
        private readonly VkDbContext _context;
        private readonly DtoResult<IEnumerable<StudentDto>> _response = new DtoResult<IEnumerable<StudentDto>>();

        public GetStudentsProgressRequestHandler(VkDbContext context)
        {
            _context = context;
        }

        public async Task<DtoResult<IEnumerable<StudentDto>>> Handle(GetStudentsProgressRequest request, CancellationToken cancellationToken)
        {
            var org = await _context.Organizations
                .Include(u => u.Students)
                .ThenInclude(s => s.StudentUser)
                .FirstOrDefaultAsync(u => u.Id == request.OrgId, cancellationToken);

            if (org == null)
            {
                _response.AddError(nameof(request.OrgId), $"Could not find the org with id of {request.OrgId}.");
                return _response;
            }

            var result = new List<StudentDto>();

            foreach (var s in org.Students)
            {
                var dto = new StudentDto(s.StudentUser);
                var studentDataService = new StudentDataService(_context, s.VkUserId);
                dto.SetProgressCounts(studentDataService);
                result.Add(dto);
            }

            if (_response.HasErrors()) return _response;
            _response.SetValue(result);

            return _response;
        }
    }
}
