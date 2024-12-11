using MediatR;
using VkCore.Dtos;
using VkCore.SharedKernel;

namespace VkCore.Requests.Organization
{
    public class GetUserOrgRequest : IRequest<DtoResult<OrgDto>>
    {
        public GetUserOrgRequest(string userId, string orgId)
        {
            UserId = userId;
            OrgId = orgId;
        }

        public string UserId { get; set; }
        public string OrgId { get; set; }
    }
}
