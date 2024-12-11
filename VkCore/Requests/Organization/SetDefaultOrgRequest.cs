using System.Collections.Generic;
using MediatR;
using VkCore.Dtos;
using VkCore.SharedKernel;

namespace VkCore.Requests.Organization
{
    public class SetDefaultOrgRequest : IRequest<DtoResult<IEnumerable<OrgDto>>>
    {
        public string OrgId { get; set; }
        public string UserId { get; set; }
    }
}
