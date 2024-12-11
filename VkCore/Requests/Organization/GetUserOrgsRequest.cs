using System.Collections.Generic;
using MediatR;
using VkCore.Dtos;
using VkCore.Models.Organization;
using VkCore.SharedKernel;

namespace VkCore.Requests.Organization
{
    public class GetUserOrgsRequest : IRequest<DtoResult<IEnumerable<OrgDto>>>
    {
        public GetUserOrgsRequest(string userId, string userType)
        {
            UserId = userId;
            UserType = userType;
        }

        public string UserId { get; set; }
        public string UserType { get; set; }
    }
}
