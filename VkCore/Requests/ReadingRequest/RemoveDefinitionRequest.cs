using MediatR;
using VkCore.Dtos;
using VkCore.SharedKernel;

namespace VkCore.Requests.ReadingRequest
{
    public class RemoveDefinitionRequest : IRequest<DtoResult<ReadingDto>>
    {
        public RemoveDefinitionRequest(string readingId, string contentItemId)
        {
            ReadingId = readingId;
            ContentItemId = contentItemId;
        }

        public string ReadingId { get; set; }
        public string ContentItemId { get; set; }
    }
}
