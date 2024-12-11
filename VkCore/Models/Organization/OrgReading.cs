using System;
using VkCore.SharedKernel;

namespace VkCore.Models.Organization
{
    public class OrgReading : BaseEntity
    {
        private OrgReading() { }
        public OrgReading(Org org, ReadingModel.Reading reading)
        {
            this.Id = Guid.NewGuid().ToString();
            this.OrgId = org.Id;
            this.Org = org;
            this.ReadingId = reading.Id;
            this.Reading = reading;
        }

        public string OrgId { get; set; }
        public Org Org { get; set; }

        public string ReadingId { get; set; }
        public ReadingModel.Reading Reading { get; set; }
    }
}
