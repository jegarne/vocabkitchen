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
    public class GetUserOrgsRequestHandler : IRequestHandler<GetUserOrgsRequest, DtoResult<IEnumerable<OrgDto>>>
    {
        private readonly DtoResult<IEnumerable<OrgDto>> _response = new DtoResult<IEnumerable<OrgDto>>();
        private readonly VkDbContext _context;

        public GetUserOrgsRequestHandler(
            VkDbContext context
        )
        {
            _context = context;
        }

        public Task<DtoResult<IEnumerable<OrgDto>>> Handle(GetUserOrgsRequest request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.UserId))
            {
                _response.AddError("UserId was not provided.");
                return Task.FromResult(_response);
            }

            var orgs = new List<Org>();

            switch (request.UserType.ToLower())
            {
                case UserTypes.Admin:
                    orgs = _context.Organizations
                        .Where(o => o.Admins.Any(a => a.AdminUser.Id == request.UserId))
                        .ToList();
                    break;
                case UserTypes.Teacher:
                    orgs = _context.Organizations
                        .Where(o => o.Teachers.Any(a => a.TeacherUser.Id == request.UserId))
                        .ToList();
                    break;
                case UserTypes.Student:
                    orgs = _context.Organizations
                        .Where(o => o.Students.Any(a => a.StudentUser.Id == request.UserId))
                        .ToList();
                    break;
            }

            var orgDtos = OrgDto.BuildOrgDtos(orgs).ToList();

            // set default org for teachers
            if (request.UserType == UserTypes.Teacher)
            {
                var defaultOrg = _context.OrgTeachers
                    .FirstOrDefault(o => o.VkUserId == request.UserId && o.IsDefault);
                if (defaultOrg != null)
                {
                    var defaultOrgDto = orgDtos.First(o => o.Id == defaultOrg.OrgId);
                    defaultOrgDto.IsDefault = true;
                }
            }

            _response.SetValue(orgDtos);

            return Task.FromResult(_response);
        }
    }
}
