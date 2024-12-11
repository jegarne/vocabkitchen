using MediatR;
using VkCore.Dtos;
using VkCore.SharedKernel;

namespace VkCore.Requests.TagRequest
{
    public class DeleteTagRequest : IRequest<DtoResult<TagDto>>
    {
        public DeleteTagRequest(string tagId)
        {
            TagId = tagId;
        }

        public string TagId { get; set; }
    }
}
