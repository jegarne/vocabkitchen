using MediatR;
using VkCore.Dtos;
using VkCore.Models;
using VkCore.SharedKernel;

namespace VkCore.Requests.User
{
    public class CreateUserRequest : IRequest<DtoResult<UserDto>>
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string OrganizationName { get; set; }

        public VkUser ToVkUser()
        {
            return new VkUser(this.Email, this.FirstName, this.LastName);
        }
    }
}
