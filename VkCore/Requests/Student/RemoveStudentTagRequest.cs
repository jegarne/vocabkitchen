using System.Collections.Generic;
using MediatR;
using VkCore.Dtos;
using VkCore.SharedKernel;

namespace VkCore.Requests.Student
{
    public class RemoveStudentTagRequest : IRequest<DtoResult<IEnumerable<TagDto>>>
    {
        public RemoveStudentTagRequest(string studentId, string tagId)
        {
            StudentId = studentId;
            TagId = tagId;
        }

        public string StudentId { get; set; }
        public string TagId { get; set; }
    }
}
