using MediatR;

namespace VkCore.Events.Organization
{
    public class OrgTeacherAddedEvent : INotification
    {
        public OrgTeacherAddedEvent(string orgId, string vkUserId)
        {
            OrgId = orgId;
            VkUserId = vkUserId;
        }

        public string OrgId { get; }
        public string VkUserId { get; }
    }
}
