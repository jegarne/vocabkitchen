using MediatR;

namespace VkCore.Events.Organization
{
    public class OrgAdminAddedEvent : INotification
    {
        public OrgAdminAddedEvent(string orgId, string vkUserId)
        {
            OrgId = orgId;
            VkUserId = vkUserId;
        }

        public string OrgId { get; }
        public string VkUserId { get; }
    }
}
