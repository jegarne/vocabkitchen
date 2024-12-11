using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VkCore.Constants;
using VkCore.Dtos;
using VkCore.Models.Organization;
using VkCore.Requests.Organization;
using VkCore.SharedKernel;
using VkInfrastructure.Data;

namespace VkInfrastructure.RequestHandlers.Organization
{
    public class SetDefaultOrgRequestHandler : IRequestHandler<SetDefaultOrgRequest, DtoResult<IEnumerable<OrgDto>>>
    {
        private readonly DtoResult<IEnumerable<OrgDto>> _response = new DtoResult<IEnumerable<OrgDto>>();
        private readonly VkDbContext _context;

        public SetDefaultOrgRequestHandler(
            VkDbContext context
        )
        {
            _context = context;
        }

        public Task<DtoResult<IEnumerable<OrgDto>>> Handle(SetDefaultOrgRequest request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.UserId))
            {
                _response.AddError("UserId was not provided.");
                return Task.FromResult(_response);
            }

            var orgTeachers = _context.OrgTeachers.Where(o => o.VkUserId == request.UserId).ToList();
            var oldDefault = orgTeachers.FirstOrDefault(t => t.IsDefault == true);
            oldDefault.IsDefault = false;

            var newDefault = orgTeachers.FirstOrDefault(t => t.OrgId == request.OrgId);
            newDefault.IsDefault = true;

            _context.SaveChanges();

            var orgs = new List<Org>();
            orgs = _context.Organizations
                .Where(o => o.Teachers.Any(a => a.TeacherUser.Id == request.UserId))
                .ToList();
            var orgDtos = OrgDto.BuildOrgDtos(orgs);
            orgDtos.FirstOrDefault(o => o.Id == newDefault.OrgId).IsDefault = true;

            _response.SetValue(orgDtos);

            return Task.FromResult(_response);
        }
    }
}
