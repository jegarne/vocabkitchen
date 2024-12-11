using System;
using VkCore.SharedKernel;

namespace VkCore.Models.Organization
{
    public class OrgStudent : BaseEntity
    {
        private OrgStudent() { }
        public OrgStudent(Org org, VkUser user)
        {
            this.Id = Guid.NewGuid().ToString();
            this.OrgId = org.Id;
            this.Org = org;
            this.VkUserId = user.Id;
            this.StudentUser = user;
        }

        public string OrgId { get; set; }
        public Org Org { get; set; }

        public string VkUserId { get; set; }
        public VkUser StudentUser { get; set; }
    }
}
