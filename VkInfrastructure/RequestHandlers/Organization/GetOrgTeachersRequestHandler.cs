using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using VkCore.Dtos;
using VkCore.Models.Invite;
using VkCore.Requests.Organization;
using VkCore.SharedKernel;
using VkInfrastructure.Data;

namespace VkInfrastructure.RequestHandlers.Organization
{
    public class GetOrgTeachersRequestHandler : IRequestHandler<GetOrgTeachersRequest, DtoResult<IEnumerable<OrgTeacherDto>>>
    {
        private readonly VkDbContext _context;
        private readonly DtoResult<IEnumerable<OrgTeacherDto>> _response;

        public GetOrgTeachersRequestHandler(VkDbContext context)
        {
            _context = context;
            _response = new DtoResult<IEnumerable<OrgTeacherDto>>();
        }

        public async Task<DtoResult<IEnumerable<OrgTeacherDto>>> Handle(GetOrgTeachersRequest request, CancellationToken cancellationToken)
        {
            var org = await _context.Organizations.FirstOrDefaultAsync(o => o.Admins.Any(a => a.OrgId == request.OrgId));

            if (org == null)
            {
                _response.AddError(nameof(request.OrgId), $"Could not find the organization with id of {request.OrgId}.");
                return _response;
            }

            var teacherList = new List<OrgTeacherDto>();

            var teachers = await _context.Users.Where(u => u.TeacherOrgs.Any(o => o.OrgId == request.OrgId)).ToListAsync();
            var admins = await _context.Users.Where(u => u.AdminOrgs.Any(o => o.OrgId == request.OrgId)).ToListAsync();
            teacherList.AddRange(OrgTeacherDto.FromOrgTeachers(teachers, admins, request.VkUserId));

            var invitedTeachers = await _context.Invites
                .Where(i => i.InviteType == OrgInviteType.Teacher && i.OrgId == request.OrgId).ToListAsync();
            teacherList.AddRange(OrgTeacherDto.FromTeacherInvites(invitedTeachers));

            _response.SetValue(teacherList.AsEnumerable());

            return _response;
        }
    }
}
