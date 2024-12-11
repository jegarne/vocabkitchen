using MediatR;
using VkCore.Models;
using VkCore.SharedKernel;

namespace VkCore.Requests.User
{
    public class GetUserDetailsRequest : IRequest<DtoResult<VkUser>>
    {
        public GetUserDetailsRequest(string userId)
        {
            UserId = userId;
        }

        public string UserId { get; set; }
    }
}
