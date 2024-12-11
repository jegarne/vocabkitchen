using System.Collections.Generic;
using MediatR;
using VkCore.Dtos;
using VkCore.SharedKernel;

namespace VkCore.Requests.Organization
{
    public class GetWordsProgressRequest : IRequest<DtoResult<IEnumerable<WordAttemptSummaryDto>>>
    {
        public GetWordsProgressRequest(string orgId)
        {
            OrgId = orgId;
        }

        public string OrgId { get; set; }
    }
}
