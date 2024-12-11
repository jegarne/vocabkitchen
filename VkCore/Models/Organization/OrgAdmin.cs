using System;
using VkCore.SharedKernel;

namespace VkCore.Models.Organization
{
    public class OrgAdmin : BaseEntity
    {
        private OrgAdmin() { }
        public OrgAdmin(Org org, VkUser user)
        {
            this.Id = Guid.NewGuid().ToString();
            this.OrgId = org.Id;
            this.Org = org;
            this.VkUserId = user.Id;
            this.AdminUser = user;
        }

        public string OrgId { get; set; }
        public Org Org { get; set; }

        public string VkUserId { get; set; }
        public VkUser AdminUser { get; set; }
    }
}
