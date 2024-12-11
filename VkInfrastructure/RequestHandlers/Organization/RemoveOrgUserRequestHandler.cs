using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VkCore.Constants;
using VkCore.Models.Organization;
using VkCore.Requests.Organization;
using VkCore.SharedKernel;
using VkInfrastructure.Data;

namespace VkInfrastructure.RequestHandlers.Organization
{
    public class RemoveOrgUserRequestHandler : IRequestHandler<RemoveOrgUserRequest, DtoResult<Org>>
    {
        private readonly VkDbContext _context;
        private readonly DtoResult<Org> _response;

        public RemoveOrgUserRequestHandler(VkDbContext context)
        {
            _context = context;
            _response = new DtoResult<Org>();
        }

        public Task<DtoResult<Org>> Handle(RemoveOrgUserRequest request, CancellationToken cancellationToken)
        {
            var org = _context.Organizations.Find(request.OrgId);

            if (org == null)
            {
                _response.AddError(nameof(request.OrgId), $"Could not find the organization with id of {request.OrgId}.");
                return Task.FromResult(_response);
            }

            switch (request.UserType.ToLower())
            {
                case UserTypes.Admin:
                    org.RemoveAdmin(request.VkUserId, _context);
                    break;
                case UserTypes.Teacher:
                    org.RemoveTeacher(request.VkUserId, _context);
                    break;
                case UserTypes.Student:
                    org.RemoveStudent(request.VkUserId, _context);
                    break;
            }

            _response.AddErrors(org);
            if (_response.HasErrors()) Task.FromResult(_response);

            _context.Organizations.Update(org);
            _context.SaveChanges();
            _response.SetValue(org);

            return Task.FromResult(_response);
        }
    }
}
