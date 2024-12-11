using System;
using VkCore.SharedKernel;

namespace VkCore.Models.Organization
{
    public class OrgTeacher : BaseEntity
    {
        private OrgTeacher() { }
        public OrgTeacher(Org org, VkUser user)
        {
            this.Id = Guid.NewGuid().ToString();
            this.OrgId = org.Id;
            this.Org = org;
            this.VkUserId = user.Id;
            this.TeacherUser = user;
        }

        // default org for saving profiled readings
        public bool IsDefault { get; set; }

        public string OrgId { get; set; }
        public Org Org { get; set; }

        public string VkUserId { get; set; }
        public VkUser TeacherUser { get; set; }
    }
}
