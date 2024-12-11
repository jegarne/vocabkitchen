using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using VkCore.Dtos;
using VkCore.Models.Invite;
using VkCore.Models.Organization;
using VkCore.Requests.Organization;
using VkCore.SharedKernel;
using VkInfrastructure.Data;

namespace VkInfrastructure.RequestHandlers.Organization
{
    public class GetOrgStudentsRequestHandler : IRequestHandler<GetOrgStudentsRequest, DtoResult<IEnumerable<OrgStudentDto>>>
    {
        private readonly VkDbContext _context;
        private readonly DtoResult<IEnumerable<OrgStudentDto>> _response;

        public GetOrgStudentsRequestHandler(VkDbContext context)
        {
            _context = context;
            _response = new DtoResult<IEnumerable<OrgStudentDto>>();
        }

        public async Task<DtoResult<IEnumerable<OrgStudentDto>>> Handle(GetOrgStudentsRequest request, CancellationToken cancellationToken)
        {
            var org = await _context.FindAsync<Org>(request.OrgId);
            if (org == null)
            {
                _response.AddError(nameof(request.OrgId), $"Could not find the organization with id of {request.OrgId}.");
                return _response;
            }

            var studentList = new List<OrgStudentDto>();

            var students = await _context.Users.Where(u => u.StudentOrgs.Any(o => o.OrgId == request.OrgId)).ToListAsync();
            studentList.AddRange(OrgStudentDto.FromOrgStudents(students));

            var invitedStudents = await _context.Invites
                .Where(i => i.InviteType == OrgInviteType.Student && i.OrgId == request.OrgId).ToListAsync();
            studentList.AddRange(OrgStudentDto.FromStudentInvites(invitedStudents));

            _response.SetValue(studentList.AsEnumerable());

            return _response;
        }
    }
}
