using System.Collections.Generic;
using MediatR;
using VkCore.Models.TagModel;
using VkCore.SharedKernel;

namespace VkCore.Requests.Organization
{
    public class AddOrgTagRequest : IRequest<DtoResult<IEnumerable<Tag>>>
    {
        public AddOrgTagRequest(string orgId, string vkUserId, string tagValue)
        {
            OrgId = orgId;
            VkUserId = vkUserId;
            TagValue = tagValue;
        }

        public string OrgId { get; set; }
        public string VkUserId { get; set; }
        public string TagValue { get; set; }
    }
}
