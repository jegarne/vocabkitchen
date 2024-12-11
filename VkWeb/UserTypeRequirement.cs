using Microsoft.AspNetCore.Authorization;

namespace VkWeb
{
    public class UserTypeRequirement : IAuthorizationRequirement
    {
        public UserTypeRequirement(string userType)
        {
            UserType = userType;
        }

        public string UserType { get; set; }
    }
}