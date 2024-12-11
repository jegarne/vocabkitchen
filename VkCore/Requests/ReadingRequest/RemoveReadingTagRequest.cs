using System.Collections.Generic;
using MediatR;
using VkCore.Dtos;
using VkCore.SharedKernel;

namespace VkCore.Requests.ReadingRequest
{
    public class RemoveReadingTagRequest : IRequest<DtoResult<IEnumerable<TagDto>>>
    {
        public RemoveReadingTagRequest(string readingId, string tagId)
        {
            ReadingId = readingId;
            TagId = tagId;
        }

        public string ReadingId { get; set; }
        public string TagId { get; set; }
    }
}
