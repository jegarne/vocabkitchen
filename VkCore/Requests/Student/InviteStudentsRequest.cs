using System.Collections.Generic;
using MediatR;
using VkCore.SharedKernel;

namespace VkCore.Requests.Student
{
    public class InviteStudentsRequest : IRequest<DtoResult<string>>
    {
        private InviteStudentsRequest() { }

        public InviteStudentsRequest(string orgId, IEnumerable<string> emails)
        {
            OrgId = orgId;
            Emails = emails;
        }

        public string OrgId { get; set; }
        public IEnumerable<string> Emails { get; set; }
    }
}
