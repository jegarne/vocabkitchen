using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VkCore.Constants;
using VkCore.Interfaces;
using VkCore.Models;
using VkCore.Models.Organization;
using VkCore.Requests.Organization;
using VkCore.SharedKernel;
using VkInfrastructure.Data;

namespace VkInfrastructure.RequestHandlers.Organization
{
    public class AddOrgUserRequestHandler : IRequestHandler<AddOrgUserRequest, DtoResult<Org>>
    {
        private readonly VkDbContext _context;
        private readonly DtoResult<Org> _response;
        private readonly ITagService _tagService;

        public AddOrgUserRequestHandler(VkDbContext context, ITagService tagService)
        {
            _context = context;
            _response = new DtoResult<Org>();
            _tagService = tagService;
        }

        public async Task<DtoResult<Org>> Handle(AddOrgUserRequest request, CancellationToken cancellationToken)
        {
            var org = _context.Organizations.Find(request.OrgId);

            if (org == null)
            {
                _response.AddError(nameof(request.OrgId), $"Could not find the organization with id of {request.OrgId}.");
                return _response;
            }

            var user = _context.Find<VkUser>(request.VkUserId);
            if (user == null)
            {
                _response.AddError(nameof(request.VkUserId), $"Could not find the user with id of {request.VkUserId}.");
                return _response;
            }

            switch (request.UserType.ToLower())
            {
                case UserTypes.Admin:
                    org.AddAdmin(user, _context);
                    break;
                case UserTypes.Teacher:
                    org.AddTeacher(user, _context);
                    break;
                case UserTypes.Student:
                    {
                        org.AddStudent(user, _context);
                        var defaultTag = await _tagService.GetDefaultTagAsync(request.OrgId);
                        defaultTag?.AddUser(user.Id, _context);
                        break;
                    }
            }


            _response.AddErrors(org);
            if (_response.HasErrors()) return _response;

            await _context.SaveChangesAsync(cancellationToken);

            _response.SetValue(org);

            return _response;
        }
    }
}
