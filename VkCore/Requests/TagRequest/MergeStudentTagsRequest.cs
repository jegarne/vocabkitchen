using MediatR;
using System.Collections.Generic;
using VkCore.Dtos;
using VkCore.SharedKernel;

namespace VkCore.Requests.TagRequest
{
    public class MergeStudentTagsRequest : IRequest<DtoResult<IEnumerable<StudentDto>>>
    {
        public MergeStudentTagsRequest()
        {
        }

        public MergeStudentTagsRequest(string orgId, string tagId, IEnumerable<string> studentIds)
        {
            OrgId = orgId;
            TagId = tagId;
            StudentIds = studentIds;
        }

        public string OrgId { get; set; }
        public string TagId { get; set; }
        public IEnumerable<string> StudentIds { get; set; }
    }
}
