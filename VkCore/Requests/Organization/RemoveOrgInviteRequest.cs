using MediatR;
using VkCore.Models.Organization;
using VkCore.SharedKernel;

namespace VkCore.Requests.Organization
{
    public class RemoveOrgInviteRequest : IRequest<DtoResult<Org>>
    {
        public RemoveOrgInviteRequest(string orgId, string email)
        {
            OrgId = orgId;
            Email = email;
        }

        public string OrgId { get; set; }
        public string Email { get; set; }
    }
}
