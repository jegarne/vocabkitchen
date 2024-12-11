using AutoFixture;
using AutoFixture.AutoMoq;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using VkCore.Interfaces;
using VkCore.Models;
using VkCore.Models.Organization;
using VkCore.Requests.Student;
using VkInfrastructure.Data;
using VkInfrastructure.RequestHandlers.Student;
using Xunit;

namespace VkInfrastructure.Test.Requests.Student
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
        private readonly Mock<IVkConfig> _config;


        public InviteStudentsRequestHandlerShould()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization()); ;
            _context = DbContextBuilder.GetContext();
            _mediator = new Mock<IMediator>();
            _emailSender = new Mock<IEmailSender>();
            _httpContext = new Mock<IHttpContextAccessor>();
            _userManager = DbContextBuilder.GetMockUserManager();
            _token = new CancellationToken(false);
            _config = new Mock<IVkConfig>();

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

            var dto = _fixture.Create<InviteStudentsRequest>();
            dto.OrgId = "bad-id";

            var sut = new InviteStudentsRequestHandler(_context, _emailSender.Object, _httpContext.Object, _config.Object);
            var result = await sut.Handle(dto, _token);

            Assert.Equal($"Could not find the organization with id of {dto.OrgId}.", result.GetErrors()["OrgId"][0]);
        }

        [Fact]
        public async Task NotCreateDuplicateInvites()
        {
            var inviteEmail = "foo@bar.com";
            var org = _fixture.Create<Org>();
            org.AddStudentInvite(inviteEmail);
            _context.Add(org);
            _context.SaveChanges();

            var dto = new InviteStudentsRequest(org.Id, new List<string>() { inviteEmail });

            var sut = new InviteStudentsRequestHandler(_context, _emailSender.Object, _httpContext.Object, _config.Object);
            var result = await sut.Handle(dto, _token);

            Assert.Equal($"{inviteEmail} was already invited to this organization.", result.GetErrors()[string.Empty][0]);
        }

        [Fact]
        public async Task NotAddDuplicateStudents()
        {
            var newUser = _fixture.Create<VkUser>();
            _context.Add(newUser);
            var org = _fixture.Create<Org>();
            org.AddStudent(newUser);
            _context.Add(org);
            _context.SaveChanges();

            var dto = new InviteStudentsRequest(org.Id, new List<string>() { newUser.Email });

            var sut = new InviteStudentsRequestHandler(_context, _emailSender.Object, _httpContext.Object, _config.Object);
            var result = await sut.Handle(dto, _token);

            Assert.Equal($"{newUser.Email} is already a student in this organization.", result.GetErrors()[string.Empty][0]);
        }

        [Fact]
        public async Task AddNewUsersToInviteCollection()
        {
            var newUser = _fixture.Create<VkUser>();
            var org = _fixture.Create<Org>();
            _context.Add(org);
            _context.SaveChanges();

            var dto = new InviteStudentsRequest(org.Id, new List<string>() { newUser.Email });

            var sut = new InviteStudentsRequestHandler(_context, _emailSender.Object, _httpContext.Object, _config.Object);
            var result = await sut.Handle(dto, _token);

            Assert.Contains(newUser.Email, org.GetStudentInvites().Select(i => i.Email));
        }

        [Fact]
        public async Task AddExistingUsersToStudentsCollection()
        {
            var newUser = _fixture.Create<VkUser>();
            _context.Add(newUser);
            var org = _fixture.Create<Org>();
            _context.Add(org);
            _context.SaveChanges();

            var dto = new InviteStudentsRequest(org.Id, new List<string>() { newUser.Email });

            var sut = new InviteStudentsRequestHandler(_context, _emailSender.Object, _httpContext.Object, _config.Object);
            var result = await sut.Handle(dto, _token);

            Assert.Contains(newUser.Id, org.Students.Select(i => i.VkUserId));
        }

        [Fact]
        public async Task NotSendInvitesWhenAboveStudentLimit()
        {
            var newUser1 = _fixture.Create<VkUser>();
            var newUser2 = _fixture.Create<VkUser>();
            var newUser3 = _fixture.Create<VkUser>();
            _context.Add(newUser1);
            _context.Add(newUser2);
            _context.Add(newUser3);
            var org = _fixture.Create<Org>();
            org.StudentLimit = 1;
            _context.Add(org);
            _context.SaveChanges();

            _config.Setup(x => x.DefaultStudentLimit).Returns(2);

            var dto = new InviteStudentsRequest(org.Id, new List<string>() { newUser1.Email, newUser2.Email, newUser3.Email });

            var sut = new InviteStudentsRequestHandler(_context, _emailSender.Object, _httpContext.Object, _config.Object);
            var result = await sut.Handle(dto, _token);

            var errors = result.GetErrors();
            Assert.Contains("You're attempting to add 3 new students.", errors[""].First());
            Assert.DoesNotContain(newUser1.Id, org.Students.Select(i => i.VkUserId));
            _emailSender.Verify(x => 
                x.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), 
                Times.Never);
        }
    }
}
