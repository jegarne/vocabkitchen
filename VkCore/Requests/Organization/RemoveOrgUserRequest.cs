using MediatR;
using VkCore.Models.Organization;
using VkCore.SharedKernel;

namespace VkCore.Requests.Organization
{
    public class RemoveOrgUserRequest : IRequest<DtoResult<Org>>
    {
        public RemoveOrgUserRequest(string userType, string orgId, string vkUserId)
        {
            UserType = userType;
            OrgId = orgId;
            VkUserId = vkUserId;
        }

        public string UserType { get; set; }
        public string OrgId { get; set; }
        public string VkUserId { get; set; }
    }
}
