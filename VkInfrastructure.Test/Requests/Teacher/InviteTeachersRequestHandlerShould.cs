using AutoFixture;
using AutoFixture.AutoMoq;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VkCore.Models;
using VkCore.Models.Organization;
using VkCore.Requests.Teacher;
using VkInfrastructure.Data;
using VkInfrastructure.RequestHandlers.Teacher;
using Xunit;

namespace VkInfrastructure.Test.Requests.Teacher
{
    public class InviteStudentsRequestHandlerShould
    {
        private readonly VkDbContext _context;
        private readonly Mock<IMediator> _mediator;
        private readonly Mock<UserManager<VkUser>> _userManager;
        private readonly CancellationToken _token;
        private readonly IFixture _fixture;
        private readonly Mock<IEmailSender> _emailSender;
        private readonly Mock<IHttpContextAccessor> _httpContext;

        public InviteStudentsRequestHandlerShould()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization()); ;
            _context = DbContextBuilder.GetContext();
            _mediator = new Mock<IMediator>();
            _emailSender = new Mock<IEmailSender>();
            _httpContext = new Mock<IHttpContextAccessor>();
            _userManager = DbContextBuilder.GetMockUserManager();
            _token = new CancellationToken(false);

            var context = new DefaultHttpContext();
            _httpContext.Setup(_ => _.HttpContext).Returns(context);
        }

        [Fact]
        public async Task LogOrgNotFoundError()
        {
            var newUser = _fixture.Create<VkUser>();
            _context.Add(newUser);
            var org = _fixture.Create<Org>();
            _context.Add(org);
            _context.SaveChanges();

            var dto = _fixture.Create<InviteTeachersRequest>();
            dto.OrgId = "bad-id";

            var sut = new InviteTeachersRequestHandler(_context, _emailSender.Object, _httpContext.Object);
            var result = await sut.Handle(dto, _token);

            Assert.Equal($"Could not find the organization with id of {dto.OrgId}.", result.GetErrors()["OrgId"][0]);
        }

        [Fact]
        public async Task NotCreateDuplicateInvites()
        {
            var inviteEmail = "foo@bar.com";
            var org = _fixture.Create<Org>();
            org.AddTeacherInvite(inviteEmail);
            _context.Add(org);
            _context.SaveChanges();

            var dto = new InviteTeachersRequest(org.Id, new List<string>() { inviteEmail });

            var sut = new InviteTeachersRequestHandler(_context, _emailSender.Object, _httpContext.Object);
            var result = await sut.Handle(dto, _token);

            Assert.Equal($"{inviteEmail} was already invited to this organization.", result.GetErrors()[string.Empty][0]);
        }

        [Fact]
        public async Task NotAddDuplicateTeachers()
        {
            var newUser = _fixture.Create<VkUser>();
            _context.Add(newUser);
            var org = _fixture.Create<Org>();
            org.AddTeacher(newUser);
            _context.Add(org);
            _context.SaveChanges();

            var dto = new InviteTeachersRequest(org.Id, new List<string>() { newUser.Email });

            var sut = new InviteTeachersRequestHandler(_context, _emailSender.Object, _httpContext.Object);
            var result = await sut.Handle(dto, _token);

            Assert.Equal($"{newUser.Email} is already a teacher in this organization.", result.GetErrors()[string.Empty][0]);
        }

        [Fact]
        public async Task AddNewUsersToInviteCollection()
        {
            var newUser = _fixture.Create<VkUser>();
            var org = _fixture.Create<Org>();
            _context.Add(org);
            _context.SaveChanges();

            var dto = new InviteTeachersRequest(org.Id, new List<string>() { newUser.Email });

            var sut = new InviteTeachersRequestHandler(_context, _emailSender.Object, _httpContext.Object);
            var result = await sut.Handle(dto, _token);

            Assert.Contains(newUser.Email, org.GetTeacherInvites().Select(i => i.Email));
        }

        [Fact]
        public async Task AddExistingUsersToTeachersCollection()
        {
            var newUser = _fixture.Create<VkUser>();
            _context.Add(newUser);
            var org = _fixture.Create<Org>();
            _context.Add(org);
            _context.SaveChanges();

            var dto = new InviteTeachersRequest(org.Id, new List<string>() { newUser.Email });

            var sut = new InviteTeachersRequestHandler(_context, _emailSender.Object, _httpContext.Object);
            var result = await sut.Handle(dto, _token);

            Assert.Contains(newUser.Id, org.Teachers.Select(i => i.VkUserId));
        }
    }
}
