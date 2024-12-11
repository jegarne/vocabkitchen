using System.Collections.Generic;
using MediatR;
using VkCore.Dtos;
using VkCore.SharedKernel;

namespace VkCore.Requests.TagRequest
{
    public class GetTaggedStudentsRequest : IRequest<DtoResult<IEnumerable<StudentDto>>>
    {
        public GetTaggedStudentsRequest(string orgId, string tagId)
        {
            OrgId = orgId;
            TagId = tagId;
        }

        public string OrgId { get; set; }
        public string TagId { get; set; }
    }
}
