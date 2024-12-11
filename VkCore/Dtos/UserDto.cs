using VkCore.Models;
using VkCore.Models.Organization;

namespace VkCore.Dtos
{
    public class UserDto
    {
        public UserDto(VkUser user)
        {
            Id = user.Id;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Email = user.Email;
        }

        public UserDto(VkUser user, Org org)
        {
            Id = user.Id;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Email = user.Email;
            OrgId = org.Id;
            OrgName = org.Name;
        }

        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string OrgId { get; set; }
        public string OrgName { get; set; }
    }
}
