using System.Collections.Generic;
using MediatR;
using VkCore.Dtos;
using VkCore.SharedKernel;

namespace VkCore.Requests.Student
{
    public class GetUnkownWordsRequest : IRequest<DtoResult<IEnumerable<StudentWordDto>>>
    {
        public GetUnkownWordsRequest(string userId, string readingId)
        {
            UserId = userId;
            ReadingId = readingId;
        }

        public string UserId { get; set; }
        public string ReadingId { get; set; }
    }
}
