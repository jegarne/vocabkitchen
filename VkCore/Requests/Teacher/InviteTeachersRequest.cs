using System.Collections.Generic;
using MediatR;
using VkCore.SharedKernel;

namespace VkCore.Requests.Teacher
{
    public class InviteTeachersRequest : IRequest<DtoResult<string>>
    {
        private InviteTeachersRequest() { }

        public InviteTeachersRequest(string orgId, IEnumerable<string> emails)
        {
            OrgId = orgId;
            Emails = emails;
        }

        public string OrgId { get; set; }
        public IEnumerable<string> Emails { get; set; }
    }
}
