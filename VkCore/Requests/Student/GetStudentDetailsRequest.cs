using MediatR;
using VkCore.Dtos;
using VkCore.SharedKernel;

namespace VkCore.Requests.Student
{
    public class GetStudentDetailsRequest : IRequest<DtoResult<StudentDto>>
    {
        public GetStudentDetailsRequest(string id)
        {
            StudentId = id;
        }

        public string StudentId { get; set; }
    }
}
