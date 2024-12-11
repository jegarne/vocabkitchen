using MediatR;
using VkCore.SharedKernel;
using VkCore.Models.Organization;

namespace VkCore.Requests.Organization
{
    public class AcceptInviteRequest : IRequest<DtoResult<Org>>
    {
        public AcceptInviteRequest()
        { }

        public AcceptInviteRequest(string userType, string orgId, string vkUserId)
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
