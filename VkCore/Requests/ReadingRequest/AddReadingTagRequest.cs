using System.Collections.Generic;
using MediatR;
using VkCore.Dtos;
using VkCore.SharedKernel;

namespace VkCore.Requests.ReadingRequest
{
    public class AddReadingTagRequest : IRequest<DtoResult<IEnumerable<TagDto>>>
    {
        public string ReadingId { get; set; }
        public string TagId { get; set; }
    }
}
