using System.Collections.Generic;
using MediatR;
using VkCore.Dtos;
using VkCore.SharedKernel;

namespace VkCore.Requests.Student
{
    public class GetPretestWordsRequest : IRequest<DtoResult<IEnumerable<StudentWordDto>>>
    {
        public GetPretestWordsRequest(string userId, string readingId)
        {
            UserId = userId;
            ReadingId = readingId;
        }

        public string UserId { get; set; }
        public string ReadingId { get; set; }
    }
}
