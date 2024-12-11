using AutoFixture;
using AutoFixture.AutoMoq;
using MediatR;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using VkCore.Constants;
using VkCore.Interfaces;
using VkCore.Models;
using VkCore.Models.Organization;
using VkCore.Requests.Organization;
using VkInfrastructure.RequestHandlers.Organization;
using VkInfrastructure.Services;
using Xunit;

namespace VkInfrastructure.Test.Requests.Organization
{
    public class AddOrgUserRequestHandlerShould
    {
        private readonly Mock<IMediator> _mediator;

        public AddOrgUserRequestHandlerShould()
        {
            _mediator = new Mock<IMediator>();
        }

        [Fact]
        public void AddNewAdminUser()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization()); ;
            var context = DbContextBuilder.GetContext();
            var newUser = fixture.Create<VkUser>();
            context.Add(newUser);
            var org = fixture.Create<Org>();
            context.Add(org);
            context.SaveChanges();

            var dto = new AddOrgUserRequest(UserTypes.Admin, org.Id, newUser.Id);
            var token = fixture.Create<CancellationToken>();

            var sut = new AddOrgUserRequestHandler(context, new TagService(context, _mediator.Object));
            var result = sut.Handle(dto, token);

            var updatedOrg = context.Find<Org>(org.Id);
            Assert.Contains(updatedOrg.Admins, a => a.AdminUser.Id == newUser.Id);
        }

        [Fact]
        public void AddNewTeacher()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization()); ;
            var context = DbContextBuilder.GetContext();
            var newUser = fixture.Create<VkUser>();
            context.Add(newUser);
            var org = fixture.Create<Org>();
            context.Add(org);
            context.SaveChanges();

            var dto = new AddOrgUserRequest(UserTypes.Teacher, org.Id, newUser.Id);
            var token = fixture.Create<CancellationToken>();

            var sut = new AddOrgUserRequestHandler(context, new TagService(context, _mediator.Object));
            var result = sut.Handle(dto, token);

            Assert.Contains(context.Find<Org>(org.Id).Teachers, a => a.TeacherUser.Id == newUser.Id);
        }

        [Fact]
        public void AddNewStudent()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization()); ;
            var context = DbContextBuilder.GetContext();
            var newUser = fixture.Create<VkUser>();
            context.Add(newUser);
            var org = fixture.Create<Org>();
            context.Add(org);
            context.SaveChanges();

            var dto = new AddOrgUserRequest(UserTypes.Student, org.Id, newUser.Id);
            var token = fixture.Create<CancellationToken>();

            var sut = new AddOrgUserRequestHandler(context, new TagService(context, _mediator.Object));
            var result = sut.Handle(dto, token);

            Assert.Contains(context.Find<Org>(org.Id).Students, a => a.StudentUser.Id == newUser.Id);
        }

        [Fact]
        public async Task LogVkUserNotFoundErrorAsync()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization()); ;
            var context = DbContextBuilder.GetContext();
            var newUser = fixture.Create<VkUser>();
            context.Add(newUser);
            var org = fixture.Create<Org>();
            context.Add(org);
            context.SaveChanges();

            var dto = new AddOrgUserRequest(UserTypes.Student, org.Id, "bad-id");
            var token = fixture.Create<CancellationToken>();

            var sut = new AddOrgUserRequestHandler(context, new TagService(context, _mediator.Object));
            var result = await sut.Handle(dto, token);

            Assert.Equal($"Could not find the user with id of {dto.VkUserId}.", result.GetErrors()["VkUserId"][0]);
        }

        [Fact]
        public async Task LogOrgNotFoundError()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization()); ;
            var context = DbContextBuilder.GetContext();
            var newUser = fixture.Create<VkUser>();
            context.Add(newUser);
            var org = fixture.Create<Org>();
            context.Add(org);
            context.SaveChanges();

            var dto = new AddOrgUserRequest(UserTypes.Student, "bad-id", newUser.Id);
            var token = fixture.Create<CancellationToken>();

            var sut = new AddOrgUserRequestHandler(context, new TagService(context, _mediator.Object));
            var result = await sut.Handle(dto, token);

            Assert.Equal($"Could not find the organization with id of {dto.OrgId}.", result.GetErrors()["OrgId"][0]);
        }
    }
}
