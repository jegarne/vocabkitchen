using MediatR;
using VkCore.SharedKernel;

namespace VkCore.Requests.ReadingRequest
{
    public class UpdateDefinitionRequest : IRequest<DtoResult<string>>
    {
        public UpdateDefinitionRequest()
        { }

        public UpdateDefinitionRequest(string annotationId, string value)
        {
            AnnotationId = annotationId;
            Value = value;
        }

        public string AnnotationId { get; set; }
        public string Value { get; set; }
        public string UserId { get; set; }
    }
}
