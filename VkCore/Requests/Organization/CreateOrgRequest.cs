using MediatR;
using VkCore.Interfaces;
using VkCore.Models.Organization;
using VkCore.SharedKernel;

namespace VkCore.Requests.Organization
{
    public class CreateOrgRequest : IRequest<DtoResult<Org>>
    {
        public CreateOrgRequest() { }
        public CreateOrgRequest(string vkUserId, string organizationName)
        {
            VkUserId = vkUserId;
            OrganizationName = organizationName;
        }

        public string VkUserId { get; set; }
        public string OrganizationName { get; set; }

        public Models.Organization.Org ToOrganization(IVkConfig config)
        {
            return new Models.Organization.Org(this.OrganizationName, config.DefaultStudentLimit);
        }
    }
}
