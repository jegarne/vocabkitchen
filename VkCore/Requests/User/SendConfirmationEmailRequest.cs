using MediatR;
using VkCore.SharedKernel;

namespace VkCore.Requests.User
{
    public class SendConfirmationEmailRequest : IRequest<DtoResult<string>>
    {
        public SendConfirmationEmailRequest(string email)
        {
            Email = email;
        }

        public string Email { get; set; }
    }
}
