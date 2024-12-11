using VkCore.Models.Organization;

namespace VkCore.Dtos
{
    public class OrgUpdateDto
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public static OrgUpdateDto FromOrganization(Org org)
        {
            return new OrgUpdateDto()
            {
                Id = org.Id,
                Name = org.Name
            };
        }
    }
}
