using System.Collections.Generic;
using MediatR;
using VkCore.Dtos;
using VkCore.SharedKernel;

namespace VkCore.Requests.Student
{
    public class GetStudentReadingsRequest : IRequest<DtoResult<IEnumerable<ReadingDto>>>
    {
        public GetStudentReadingsRequest(string id)
        {
            StudentId = id;
        }

        public string StudentId { get; set; }
    }
}
