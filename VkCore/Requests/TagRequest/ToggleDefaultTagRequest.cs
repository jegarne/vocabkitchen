using System.Collections.Generic;
using MediatR;
using VkCore.Dtos;
using VkCore.SharedKernel;

namespace VkCore.Requests.TagRequest
{
    public class ToggleDefaultTagRequest : IRequest<DtoResult<IEnumerable<TagDto>>>
    {
        public ToggleDefaultTagRequest(string tagId)
        {
            TagId = tagId;
        }

        public string TagId { get; set; }
    }
}
