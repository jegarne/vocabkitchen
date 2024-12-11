using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using AutoFixture;
using AutoFixture.AutoMoq;
using VkCore.Constants;
using VkCore.Models;
using VkCore.Models.Organization;
using VkCore.Requests;
using VkCore.Requests.Organization;
using VkInfrastructure.RequestHandlers.Organization;
using Xunit;

namespace VkInfrastructure.Test.Requests.Organization
{
    public class RemoveOrgInviteRequestHandlerShould
    {
        [Fact]
        public void DeleteStudentInvite()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization()); ;
            var context = DbContextBuilder.GetContext();

            var org = fixture.Create<Org>();
            var inviteEmail = "foo@bar.com";
            org.AddStudentInvite(inviteEmail);
            context.Add(org);
            context.SaveChanges();
            Assert.Equal(inviteEmail, context.Find<Org>(org.Id).GetStudentInvites(context).FirstOrDefault()?.Email);

            var dto = new RemoveOrgInviteRequest(org.Id, inviteEmail);
            var token = fixture.Create<CancellationToken>();

            var sut = new RemoveOrgInviteRequestHandler(context);
            var result = sut.Handle(dto, token);

            Assert.DoesNotContain(inviteEmail, context.Find<Org>(org.Id).GetStudentInvites(context).Select(x => x.Email).ToList());
        }

        [Fact]
        public void DeleteTeacherInvite()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization()); ;
            var context = DbContextBuilder.GetContext();

            var org = fixture.Create<Org>();
            var inviteEmail = "foo@bar.com";
            org.AddTeacherInvite(inviteEmail);
            context.Add(org);
            context.SaveChanges();
            Assert.Equal(inviteEmail, context.Find<Org>(org.Id).GetTeacherInvites(context).FirstOrDefault()?.Email);

            var dto = new RemoveOrgInviteRequest(org.Id, inviteEmail);
            var token = fixture.Create<CancellationToken>();

            var sut = new RemoveOrgInviteRequestHandler(context);
            var result = sut.Handle(dto, token);

            Assert.DoesNotContain(inviteEmail, context.Find<Org>(org.Id).GetTeacherInvites(context).Select(x => x.Email).ToList());
        }
    }
}
