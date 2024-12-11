using System.Collections.Generic;
using MediatR;
using VkCore.Dtos;
using VkCore.SharedKernel;

namespace VkCore.Requests.Organization
{
    public class GetOrgStudentsRequest : IRequest<DtoResult<IEnumerable<OrgStudentDto>>>
    {
        public GetOrgStudentsRequest(string orgId)
        {
            OrgId = orgId;
        }

        public string OrgId { get; set; }
    }
}
