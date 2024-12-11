using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using VkCore.Interfaces;
using VkCore.Models;
using VkCore.Models.Organization;
using VkCore.Requests.Organization;
using VkCore.SharedKernel;
using VkInfrastructure.Data;

namespace VkInfrastructure.RequestHandlers.Organization
{
    public class CreateOrgHandler : IRequestHandler<CreateOrgRequest, DtoResult<Org>>
    {
        private readonly VkDbContext _context;
        private readonly DtoResult<Org> _response;
        private readonly IVkConfig _config;

        public CreateOrgHandler(VkDbContext context, IVkConfig config)
        {
            _context = context;
            _response = new DtoResult<Org>();
            _config = config;
        }

        public Task<DtoResult<Org>> Handle(CreateOrgRequest request, CancellationToken cancellationToken)
        {
            // create org
            var newOrg = request.ToOrganization(_config);

            // create org users
            var user = _context.Find<VkUser>(request.VkUserId);
            if (user == null)
            {
                _response.AddError(nameof(request.VkUserId), $"Could not find the user with id of {request.VkUserId}.");
                return Task.FromResult(_response);
            }
            
            newOrg.AddAdmin(user);
            newOrg.AddTeacher(user, _context);
            newOrg.AddStudent(user, _context);
            _response.AddErrors(newOrg);

            if (_response.HasErrors()) return Task.FromResult(_response);

            _context.Add(newOrg);
            _context.SaveChanges();

            _response.SetValue(newOrg);

            return Task.FromResult(_response);
        }
    }
}
