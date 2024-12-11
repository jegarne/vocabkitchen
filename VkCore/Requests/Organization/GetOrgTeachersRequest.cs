using System.Collections.Generic;
using MediatR;
using VkCore.Dtos;
using VkCore.SharedKernel;

namespace VkCore.Requests.Organization
{
    public class GetOrgTeachersRequest : IRequest<DtoResult<IEnumerable<OrgTeacherDto>>>
    {
        public GetOrgTeachersRequest(string orgId, string vkUserId)
        {
            OrgId = orgId;
            VkUserId = vkUserId;
        }

        public string OrgId { get; set; }
        public string VkUserId { get; set; }
    }
}
