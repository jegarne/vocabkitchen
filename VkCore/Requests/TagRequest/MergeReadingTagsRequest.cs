using MediatR;
using System.Collections.Generic;
using VkCore.Dtos;
using VkCore.SharedKernel;

namespace VkCore.Requests.TagRequest
{
    public class MergeReadingTagsRequest : IRequest<DtoResult<IEnumerable<ReadingDto>>>
    {
        public MergeReadingTagsRequest() { }

        public MergeReadingTagsRequest(string orgId, string tagId, IEnumerable<string> readingIds)
        {
            OrgId = orgId;
            TagId = tagId;
            ReadingIds = readingIds;
        }

        public string OrgId { get; set; }
        public string TagId { get; set; }
        public IEnumerable<string> ReadingIds { get; set; }
    }
}
