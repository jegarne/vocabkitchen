using System.Collections.Generic;
using MediatR;
using VkCore.Dtos;
using VkCore.SharedKernel;

namespace VkCore.Requests.Student
{
    public class GetStudentWordsRequest : IRequest<DtoResult<IEnumerable<StudentWordDto>>>
    {
        public GetStudentWordsRequest(string id)
        {
            StudentId = id;
        }

        public string StudentId { get; set; }
    }
}
