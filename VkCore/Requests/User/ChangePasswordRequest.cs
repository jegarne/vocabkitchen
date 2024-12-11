using MediatR;
using VkCore.SharedKernel;

namespace VkCore.Requests.User
{
    public class ChangePasswordRequest : IRequest<DtoResult<string>>
    {
        private ChangePasswordRequest()
        {
        }

        public ChangePasswordRequest(string userId, string oldPassword, string newPassword)
        {
            UserId = userId;
            OldPassword = oldPassword;
            NewPassword = newPassword;
        }

        public string UserId { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
