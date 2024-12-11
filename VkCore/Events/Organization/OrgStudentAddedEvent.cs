using MediatR;

namespace VkCore.Events.Organization
{
    public class OrgStudentAddedEvent : INotification
    {
        public OrgStudentAddedEvent(string orgId, string vkUserId)
        {
            OrgId = orgId;
            VkUserId = vkUserId;
        }

        public string OrgId { get; }
        public string VkUserId { get; }
    }
}
