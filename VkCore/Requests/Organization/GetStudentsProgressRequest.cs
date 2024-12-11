using System.Collections.Generic;
using MediatR;
using VkCore.Dtos;
using VkCore.SharedKernel;

namespace VkCore.Requests.Organization
{
    public class GetStudentsProgressRequest : IRequest<DtoResult<IEnumerable<StudentDto>>>
    {
        public GetStudentsProgressRequest(string orgId)
        {
            OrgId = orgId;
        }

        public string OrgId { get; set; }
    }
}
