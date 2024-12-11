using AutoFixture;
using AutoFixture.AutoMoq;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VkCore.Constants;
using VkCore.Models;
using VkCore.Models.Organization;
using VkCore.Requests;
using VkCore.Requests.Organization;
using VkInfrastructure.RequestHandlers.Organization;
using Xunit;

namespace VkInfrastructure.Test.Requests.Organization
{
    public class RemoveOrgUserRequestHandlerShould
    {
        [Fact]
        public void DeleteAdminUser()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization()); ;
            var context = DbContextBuilder.GetContext();
            var newUser1 = fixture.Create<VkUser>();
            context.Add(newUser1);
            var newUser2 = fixture.Create<VkUser>();
            context.Add(newUser2);

            var org = fixture.Create<Org>();
            org.AddAdmin(newUser1);
            org.AddAdmin(newUser2);
            context.Add(org);
            context.SaveChanges();
            Assert.Equal(2, context.Find<Org>(org.Id).Admins.Count());

            var dto = new RemoveOrgUserRequest(UserTypes.Admin, org.Id, newUser2.Id);
            var token = fixture.Create<CancellationToken>();

            var sut = new RemoveOrgUserRequestHandler(context);
            var result = sut.Handle(dto, token);

            Assert.DoesNotContain(context.Find<Org>(org.Id).Admins, a => a.AdminUser.Id == newUser2.Id);
        }

        [Fact]
        public void DeleteNewTeacher()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization()); ;
            var context = DbContextBuilder.GetContext();
            var newUser1 = fixture.Create<VkUser>();
            context.Add(newUser1);
            var newUser2 = fixture.Create<VkUser>();
            context.Add(newUser2);

            var org = fixture.Create<Org>();
            org.AddTeacher(newUser1);
            org.AddTeacher(newUser2);
            context.Add(org);
            context.SaveChanges();
            Assert.Equal(2, context.Find<Org>(org.Id).Teachers.Count());

            var dto = new RemoveOrgUserRequest(UserTypes.Teacher, org.Id, newUser2.Id);
            var token = fixture.Create<CancellationToken>();

            var sut = new RemoveOrgUserRequestHandler(context);
            var result = sut.Handle(dto, token);

            Assert.DoesNotContain(context.Find<Org>(org.Id).Teachers, a => a.TeacherUser.Id == newUser2.Id);
        }

        [Fact]
        public void DeleteNewStudent()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization()); ;
            var context = DbContextBuilder.GetContext();
            var newUser = fixture.Create<VkUser>();
            context.Add(newUser);
            var org = fixture.Create<Org>();
            org.AddStudent(newUser);
            context.Add(org);
            context.SaveChanges();
            Assert.Contains(context.Find<Org>(org.Id).Students, a => a.StudentUser.Id == newUser.Id);


            var dto = new RemoveOrgUserRequest(UserTypes.Student, org.Id, newUser.Id);
            var token = fixture.Create<CancellationToken>();

            var sut = new RemoveOrgUserRequestHandler(context);
            var result = sut.Handle(dto, token);

            Assert.DoesNotContain(context.Find<Org>(org.Id).Students, a => a.StudentUser.Id == newUser.Id);
        }

        [Fact]
        public async Task LogOrgNotFoundErrorAsync()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization()); ;
            var context = DbContextBuilder.GetContext();
            var newUser = fixture.Create<VkUser>();
            context.Add(newUser);
            var org = fixture.Create<Org>();
            context.Add(org);
            context.SaveChanges();

            var dto = new RemoveOrgUserRequest(UserTypes.Student, "bad-id", newUser.Id);
            var token = fixture.Create<CancellationToken>();

            var sut = new RemoveOrgUserRequestHandler(context);
            var result = await sut.Handle(dto, token);

            Assert.Equal($"Could not find the organization with id of {dto.OrgId}.", result.GetErrors()["OrgId"][0]);
        }

        [Fact]
        public async Task LogModelValidationErrors()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization()); ;
            var context = DbContextBuilder.GetContext();
            var newUser = fixture.Create<VkUser>();
            context.Add(newUser);
            var org = fixture.Create<Org>();
            org.AddAdmin(newUser);
            org.AddTeacher(newUser);
            context.Add(org);
            context.SaveChanges();

            var dto = new RemoveOrgUserRequest(UserTypes.Admin, org.Id, newUser.Id);
            var token = fixture.Create<CancellationToken>();

            var sut = new RemoveOrgUserRequestHandler(context);
            var result = await sut.Handle(dto, token);

            Assert.Equal("An organization must have at least one admin.", result.GetErrors()[String.Empty][0]);

            dto = new RemoveOrgUserRequest(UserTypes.Teacher, org.Id, newUser.Id);
            result = await sut.Handle(dto, token);

            Assert.Equal("An organization must have at least one teacher.", result.GetErrors()[String.Empty][1]);
        }
    }
}
