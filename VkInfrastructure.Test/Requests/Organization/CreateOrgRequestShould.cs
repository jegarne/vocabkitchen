using AutoFixture;
using Moq;
using VkCore.Interfaces;
using VkCore.Requests.Organization;
using Xunit;

namespace VkInfrastructure.Test.Requests.Organization
{
    public class CreateOrgRequestShould
    {
        [Fact]
        public void CreateOrg()
        {
            var config = new Mock<IVkConfig>();
            config.Setup(x => x.DefaultStudentLimit).Returns(60);

            var fixture = new Fixture();
            var sut = fixture.Create<CreateOrgRequest>();
            var result = sut.ToOrganization(config.Object);

            Assert.Equal(sut.OrganizationName, result.Name);
            Assert.Equal(60, result.StudentLimit);
        }
    }
}
