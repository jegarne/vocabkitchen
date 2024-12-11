using MediatR;
using VkCore.Dtos;
using VkCore.SharedKernel;

namespace VkCore.Requests.Organization
{
    public class GetOrgDetailRequest : IRequest<DtoResult<OrgDetailDto>>
    {
        public GetOrgDetailRequest(string orgId)
        {
            OrgId = orgId;
        }

        public string OrgId { get; set; }
    }
}
