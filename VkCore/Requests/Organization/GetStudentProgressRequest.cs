using MediatR;
using VkCore.Dtos;
using VkCore.SharedKernel;

namespace VkCore.Requests.Organization
{
    public class GetStudentProgressRequest : IRequest<DtoResult<StudentProgressDto>>
    {
        public GetStudentProgressRequest(string orgId, string studentId)
        {
            OrgId = orgId;
            StudentId = studentId;
        }

        public string OrgId { get; set; }
        public string StudentId { get; set; }
    }
}
