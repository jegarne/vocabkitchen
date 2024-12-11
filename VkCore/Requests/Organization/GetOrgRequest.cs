using MediatR;
using VkCore.Models.Organization;
using VkCore.SharedKernel;

namespace VkCore.Requests.Organization
{
    public class GetOrgRequest : IRequest<DtoResult<Org>>
    {
        public GetOrgRequest(string orgId)
        {
            OrgId = orgId;
        }

        public string OrgId { get; set; }
    }
}
