using System;
using System.IO;
using AutoFixture;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using VkCore.Interfaces;
using VkCore.Models;
using VkCore.Models.Organization;
using VkCore.Requests.Organization;
using VkCore.Requests.User;
using VkInfrastructure.Data;
using VkInfrastructure.RequestHandlers.Organization;
using Xunit;

namespace VkInfrastructure.Test.Requests.Organization
{
    public class CreateOrgRequestHandlerShould
    {
        private VkDbContext _context;
        private VkUser _exisingUser;

        public CreateOrgRequestHandlerShould()
        {
            _context = DbContextBuilder.GetContext();
            var fixture = new Fixture();
            var createUserRequest = fixture.Create<CreateUserRequest>();
            _context.Users.Add(createUserRequest.ToVkUser());
            _context.SaveChanges();
            _exisingUser = _context.Users.FirstOrDefault();
        }

        private string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        [Fact]
        public async Task CreateNewOrganization()
        {
            var fixture = new Fixture();
            var regModel = fixture.Create<CreateOrgRequest>();
            regModel.VkUserId = _exisingUser.Id;
            var token = fixture.Create<CancellationToken>();
            var config = new Mock<IVkConfig>();
            config.Setup(x => x.DefaultStudentLimit).Returns(60);

            var sut = new CreateOrgHandler(_context, config.Object);
            await sut.Handle(regModel, token);

            var newOrg = _context.Set<Org>().ToList().FirstOrDefault();
            Assert.Equal(regModel.OrganizationName, newOrg?.Name);
            Assert.NotNull(newOrg?.Id);
        }

        [Fact]
        public async Task SetRegistrationEmailAsOrgAdminUserName()
        {
            var fixture = new Fixture();
            var regModel = fixture.Create<CreateOrgRequest>();
            regModel.VkUserId = _exisingUser.Id;
            var token = fixture.Create<CancellationToken>();
            var config = new Mock<IVkConfig>();
            config.Setup(x => x.DefaultStudentLimit).Returns(60);

            var sut = new CreateOrgHandler(_context, config.Object);
            await sut.Handle(regModel,token);

            var newOrg = _context.Set<Org>().ToList().FirstOrDefault();
            Assert.Equal(_exisingUser.Email, newOrg?.Admins?.FirstOrDefault()?.AdminUser.UserName);
        }

        [Fact]
        public async Task SetUserAsNewOrgTeacherAndAdming()
        {
            var fixture = new Fixture();
            var regModel = fixture.Create<CreateOrgRequest>();
            var token = fixture.Create<CancellationToken>();
            regModel.VkUserId = _exisingUser.Id;
            var config = new Mock<IVkConfig>();
            config.Setup(x => x.DefaultStudentLimit).Returns(60);

            var sut = new CreateOrgHandler(_context, config.Object);
            await sut.Handle(regModel, token);

            var newOrg = _context.Set<Org>().ToList().FirstOrDefault();
            Assert.Equal(_exisingUser.Id, newOrg?.Admins.FirstOrDefault()?.VkUserId);
            Assert.Equal(_exisingUser.Id, newOrg?.Teachers.FirstOrDefault()?.VkUserId);

        }
    }
}
