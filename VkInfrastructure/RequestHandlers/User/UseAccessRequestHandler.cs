using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VkCore.Dtos;
using VkCore.Requests.User;
using VkCore.SharedKernel;
using VkInfrastructure.Data;

namespace VkInfrastructure.RequestHandlers.User
{
    public class UseAccessRequestHandler : IRequestHandler<UserAccessRequest, DtoResult<UserAccessDto>>
    {
        private readonly VkDbContext _context;
        private readonly DtoResult<UserAccessDto> _response = new DtoResult<UserAccessDto>();

        public UseAccessRequestHandler(VkDbContext context)
        {
            _context = context;
        }

        public async Task<DtoResult<UserAccessDto>> Handle(UserAccessRequest request, CancellationToken cancellationToken)
        {
            var dto = new UserAccessDto
            {
                AdminOrgIds = await _context.Organizations
                    .Where(o => o.Admins.Any(x => x.VkUserId == request.VkUserId))
                    .Select(x => x.Id).ToListAsync(cancellationToken),
                TeacherOrgIds = await _context.Organizations
                    .Where(o => o.Teachers.Any(x => x.VkUserId == request.VkUserId))
                    .Select(x => x.Id).ToListAsync(cancellationToken),
                StudentOrgIds = await _context.Organizations
                    .Where(o => o.Students.Any(x => x.VkUserId == request.VkUserId))
                    .Select(x => x.Id).ToListAsync(cancellationToken)
            };

            _response.SetValue(dto);

            return _response;
        }
    }
}
