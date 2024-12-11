using MediatR;
using VkCore.SharedKernel;

namespace VkCore.Requests.ReadingRequest
{
    public class DeleteReadingRequest : IRequest<DtoResult<string>>
    {
        public DeleteReadingRequest()
        { }

        public DeleteReadingRequest(string readingId)
        {
            ReadingId = readingId;
        }

        public string ReadingId { get; set; }
    }
}
