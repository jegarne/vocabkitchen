using MediatR;
using VkCore.SharedKernel;

namespace VkCore.Requests.User
{
    public class ConfirmEmailRequest : IRequest<DtoResult<string>>
    {
        public ConfirmEmailRequest(string userId, string token)
        {
            UserId = userId;
            Token = token;
        }

        public string UserId { get; set; }
        public string Token { get; set; }
    }
}
