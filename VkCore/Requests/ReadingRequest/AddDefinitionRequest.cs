using MediatR;
using VkCore.Dtos;
using VkCore.SharedKernel;

namespace VkCore.Requests.ReadingRequest
{
    public class AddDefinitionRequest : IRequest<DtoResult<ReadingDto>>
    {
        public string ReadingId { get; set; }
        public int ContentItemStartIndex { get; set; }
        public int ContentItemEndIndex { get; set; }
        public int Start { get; set; }
        public int End { get; set; }
        public string Word { get; set; }
        public string PartOfSpeech { get; set; }
        public string Definition { get; set; }
        public string Source { get; set; }
        public string UserId { get; set; }
        public string AnnotationId { get; set; }
    }
}
