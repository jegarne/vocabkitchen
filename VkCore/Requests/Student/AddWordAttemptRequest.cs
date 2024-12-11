using MediatR;
using VkCore.Dtos;
using VkCore.SharedKernel;

namespace VkCore.Requests.Student
{

    public class AddWordAttemptRequest : IRequest<DtoResult<StudentWordDto>>
    {
        public AddWordAttemptRequest() { }
        public AddWordAttemptRequest(string userId, string wordId, string annotationId, string attemptType, bool wasSuccessful)
        {
            UserId = userId;
            WordId = wordId;
            AnnotationId = annotationId;
            AttemptType = attemptType;
            WasSuccessful = wasSuccessful;
        }

        public string UserId { get; set; }
        public string WordId { get; set; }
        public string AnnotationId { get; set; }
        public string AttemptType { get; set; }
        public bool WasSuccessful { get; set; }
    }
}
