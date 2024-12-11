using MediatR;
using VkCore.SharedKernel;

namespace VkCore.Requests.User
{
    public class ResetPasswordTokenRequest : IRequest<DtoResult<string>>
    {
        public ResetPasswordTokenRequest(string email)
        {
            Email = email;
        }

        public string Email { get; set; }
    }
}
