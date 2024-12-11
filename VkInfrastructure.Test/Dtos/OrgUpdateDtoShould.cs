using AutoFixture;
using AutoFixture.AutoMoq;
using VkCore.Dtos;
using VkCore.Models.Organization;
using Xunit;

namespace VkInfrastructure.Test.Dtos
{
    public class OrgUpdateDtoShould
    {
        [Fact]
        public void CreateDtoFromOrg()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var org = fixture.Create<Org>();

            var result = OrgUpdateDto.FromOrganization(org);

            Assert.Equal(org.Id, result.Id);
            Assert.Equal(org.Name, result.Name);
        }
    }
}
