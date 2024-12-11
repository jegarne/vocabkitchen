using System.Collections.Generic;
using MediatR;
using VkCore.Dtos;
using VkCore.SharedKernel;

namespace VkCore.Requests.ReadingRequest
{
    public class UpdateReadingRequest : IRequest<DtoResult<ReadingDto>>
    {
        public UpdateReadingRequest()
        { }

        public UpdateReadingRequest(string userId, string readingId, List<ReadingEditDto> edits)
        {
            UserId = userId;
            ReadingId = readingId;
            Edits = edits;
        }

        public string UserId { get; set; }
        public string ReadingId { get; set; }
        public string Title { get; set; }
        public List<ReadingEditDto> Edits { get; set; }
    }
}
