using MediatR;
using VkCore.Dtos;
using VkCore.SharedKernel;

namespace VkCore.Requests.Student
{
    public class AddStudentWordRequest : IRequest<DtoResult<StudentWordDto>>
    {
        public AddStudentWordRequest() { }
        public AddStudentWordRequest(string userId, string wordId, string annotationId)
        {
            UserId = userId;
            WordId = wordId;
            AnnotationId = annotationId;
        }

        public string UserId { get; set; }
        public string WordId { get; set; }
        public string AnnotationId { get; set; }
    }
}
