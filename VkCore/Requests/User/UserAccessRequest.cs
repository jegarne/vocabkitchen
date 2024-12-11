using MediatR;
using VkCore.Dtos;
using VkCore.SharedKernel;

namespace VkCore.Requests.User
{
    public class UserAccessRequest : IRequest<DtoResult<UserAccessDto>>
    {
        public UserAccessRequest(string vkUserId)
        {
            VkUserId = vkUserId;
        }

        public string VkUserId { get; set; }
    }
}
