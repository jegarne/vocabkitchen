using MediatR;
using VkCore.SharedKernel;

namespace VkCore.Requests.User
{
    public class ResetPasswordRequest : IRequest<DtoResult<string>>
    {
        public ResetPasswordRequest() { }

        public ResetPasswordRequest(string userId, string token, string newPassword)
        {
            UserId = userId;
            Token = token;
            NewPassword = newPassword;
        }

        public string UserId { get; set; }
        public string Token { get; set; }
        public string NewPassword { get; set; }
    }
}
