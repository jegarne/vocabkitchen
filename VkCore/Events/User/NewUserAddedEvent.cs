using MediatR;
using VkCore.Models;

namespace VkCore.Events.User
{
    public class NewUserAddedEvent : INotification
    {
        public NewUserAddedEvent(VkUser user, bool isEmailConfirmed)
        {
            User = user;
            IsEmailConfirmed = isEmailConfirmed;
        }

        public VkUser User { get; set; }
        public bool IsEmailConfirmed { get; set; }
    }
}
