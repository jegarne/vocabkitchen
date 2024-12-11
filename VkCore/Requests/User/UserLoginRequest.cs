using MediatR;
using VkCore.SharedKernel;

namespace VkCore.Requests.User
{
    public class UserLoginRequest : IRequest<DtoResult<string>>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
