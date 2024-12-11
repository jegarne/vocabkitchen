using MediatR;
using VkCore.Dtos;
using VkCore.SharedKernel;

namespace VkCore.Requests.TagRequest
{
    public class GetTagRequest : IRequest<DtoResult<TagDto>>
    {
        public GetTagRequest(string tagId)
        {
            TagId = tagId;
        }

        public string TagId { get; set; }
    }
}
