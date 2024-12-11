using System.Collections.Generic;
using MediatR;
using VkCore.Models.TagModel;
using VkCore.SharedKernel;

namespace VkCore.Requests.Organization
{
    public class SearchOrgTagRequest : IRequest<DtoResult<IEnumerable<Tag>>>
    {
        public SearchOrgTagRequest(string orgId, string tagValue)
        {
            OrgId = orgId;
            TagValue = tagValue;
        }

        public string OrgId { get; set; }
        public string TagValue { get; set; }
    }
}
