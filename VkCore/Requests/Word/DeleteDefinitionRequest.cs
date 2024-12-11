using System.Collections.Generic;
using MediatR;
using VkCore.Dtos;
using VkCore.SharedKernel;

namespace VkCore.Requests.Word
{
    public class DeleteDefinitionRequest : IRequest<DtoResult<DefinitionDto>>
    {
        public DeleteDefinitionRequest(string annotationId, string requestingUserId)
        {
            AnnotationId = annotationId;
            RequestingUserId = requestingUserId;
        }

        public string AnnotationId { get; set; }
        public string RequestingUserId { get; set; }
    }
}
