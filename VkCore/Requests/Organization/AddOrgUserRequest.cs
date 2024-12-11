using MediatR;
using VkCore.Models.Organization;
using VkCore.SharedKernel;

namespace VkCore.Requests.Organization
{
    public class AddOrgUserRequest : IRequest<DtoResult<Org>>
    {
        public AddOrgUserRequest()
        {}

        public AddOrgUserRequest(string userType, string orgId, string vkUserId)
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
