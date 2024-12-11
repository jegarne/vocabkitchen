using MediatR;
using VkCore.Dtos;
using VkCore.SharedKernel;

namespace VkCore.Requests.ReadingRequest
{
    public class GetReadingRequest : IRequest<DtoResult<ReadingDto>>
    {
        public GetReadingRequest()
        { }

        public GetReadingRequest(string readingId, string studentId)
        {
            ReadingId = readingId;
            StudentId = studentId;
        }

        public string ReadingId { get; set; }
        public string StudentId { get; set; }
    }
}
