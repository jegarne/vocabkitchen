using System.Collections.Generic;
using MediatR;
using VkCore.Dtos;
using VkCore.SharedKernel;

namespace VkCore.Requests.Student
{
    public class AddStudentTagRequest : IRequest<DtoResult<IEnumerable<TagDto>>>
    {
        public string StudentId { get; set; }
        public string TagId { get; set; }
    }
}
