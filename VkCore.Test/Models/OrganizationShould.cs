using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoFixture;
using AutoFixture.AutoMoq;
using Microsoft.EntityFrameworkCore;
using Moq;
using VkCore.Interfaces;
using VkCore.Models;
using VkCore.Models.Invite;
using VkCore.Models.Organization;
using VkCore.Models.ReadingModel;
using Xunit;

namespace VkCore.Test.Models
{
    public class OrganizationShould
    {
        [Fact]
        public void AddAndRemoveAdmins()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var vkUser = fixture.Create<VkUser>();
            var sut = fixture.Create<Org>();

            sut.AddAdmin(vkUser);
            Assert.Contains(sut.Admins, a => a.AdminUser.Id == vkUser.Id);

            var vkUser2 = fixture.Create<VkUser>();
            sut.AddAdmin(vkUser2);

            sut.RemoveAdmin(vkUser.Id);
            Assert.DoesNotContain(sut.Admins, a => a.AdminUser.Id == vkUser.Id);
        }

        [Fact]
        public void RequireOrgsToHaveOneAdmin()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var vkUser = fixture.Create<VkUser>();
            var sut = fixture.Create<Org>();
            sut.AddAdmin(vkUser);

            foreach (var admin in sut.Admins)
            {
                sut.RemoveAdmin(admin.VkUserId);
            }

            Assert.Single(sut.Admins);
            Assert.Contains(sut.Errors, a => a == "An organization must have at least one admin.");
        }

        [Fact]
        public void AddAndRemoveTeachers()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var vkUser = fixture.Create<VkUser>();
            var sut = fixture.Create<Org>();

            sut.AddTeacher(vkUser);
            Assert.Contains(sut.Teachers, a => a.TeacherUser.Id == vkUser.Id);

            var vkUser2 = fixture.Create<VkUser>();
            sut.AddTeacher(vkUser2);
            sut.AddAdmin(vkUser2);

            sut.RemoveTeacher(vkUser.Id);
            Assert.DoesNotContain(sut.Teachers, a => a.TeacherUser.Id == vkUser.Id);
        }

        [Fact]
        public void RequireOrgsToHaveOneTeacher()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var vkUser = fixture.Create<VkUser>();
            var sut = fixture.Create<Org>();
            sut.AddTeacher(vkUser);

            foreach (var teacher in sut.Teachers)
            {
                sut.RemoveTeacher(teacher.VkUserId);
            }

            Assert.Single(sut.Teachers);
            Assert.Contains(sut.Errors, a => a == "An organization must have at least one teacher.");
        }

        [Fact]
        public void AddAndRemoveStudents()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var vkUser = fixture.Create<VkUser>();
            var sut = fixture.Create<Org>();

            sut.AddStudent(vkUser);
            Assert.Contains(sut.Students, a => a.StudentUser.Id == vkUser.Id);

            sut.RemoveStudent(vkUser.Id);
            Assert.DoesNotContain(sut.Students, a => a.StudentUser.Id == vkUser.Id);
        }
        
        [Fact]
        public void AddReading()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var reading = new Reading("title", "text");
            var sut = fixture.Create<Org>();

            sut.AddReading(reading);

            Assert.Contains(sut.Readings, a => a.Reading.Id == reading.Id);
        }

        [Fact]
        public void RequireReadingsToHaveATitle()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var reading = new Reading("title", "text");
            reading.Title = "";
            var sut = fixture.Create<Org>();

            sut.AddReading(reading);

            Assert.Contains(sut.Errors, a => a == "A reading must have a title.");
            Assert.DoesNotContain(sut.Readings, a => a.Reading.Id == reading.Id);
        }

        [Fact]
        public void RemoveReading()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var reading = new Reading("title", "text");
            var sut = fixture.Create<Org>();

            sut.AddReading(reading);
            sut.RemoveReading(reading.Id);

            Assert.DoesNotContain(sut.Readings, a => a.Reading.Id == reading.Id);
        }

        [Fact]
        public void UpsertTagValues()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var tagValue = "test";
            var sut = fixture.Create<Org>();

            sut.UpsertTag(tagValue);
            sut.UpsertTag(tagValue);
            sut.UpsertTag(tagValue);

            Assert.Single(sut.Tags, a => a.Value == tagValue);
        }

        [Fact]
        public void NotAddEmptyTags()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var sut = fixture.Create<Org>();

            sut.UpsertTag("");

            Assert.Contains(sut.Errors, a => a == "A tag cannot be empty.");
        }

        [Fact]
        public void NotEditEmptyTags()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var sut = fixture.Create<Org>();

            sut.EditTag("old", "");

            Assert.Contains(sut.Errors, a => a == "A tag cannot be empty.");
        }

        [Fact]
        public void NotCreateDuplicateTags()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var tag1 = "tag1";
            var tag2 = "tag2";
            var sut = fixture.Create<Org>();

            sut.UpsertTag(tag1);
            sut.UpsertTag(tag2);

            sut.EditTag(tag2, tag1);

            Assert.Contains(sut.Errors, a => a == "A tag with that name already exists.");
        }

        [Fact]
        public void NotEditTagsThatDoNotExist()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var tag1 = "tag1";
            var tag2 = "tag2";
            var sut = fixture.Create<Org>();

            sut.EditTag(tag2, tag1);

            Assert.Contains(sut.Errors, a => a == "Could not find this tag.");
        }

        [Fact]
        public void EditTagValues()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var tag1 = "tag1";
            var tag2 = "tag2";
            var sut = fixture.Create<Org>();

            sut.UpsertTag(tag1);
            sut.EditTag(tag1, tag2);

            Assert.DoesNotContain(sut.Tags, a => a.Value == tag1);
            Assert.Single(sut.Tags, a => a.Value == tag2);
        }

        [Fact]
        public void GetFilteredTagValues()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var tagValue1 = "aa";
            var tagValue2 = "ab";
            var tagValue3 = "abc";
            var sut = fixture.Create<Org>();

            sut.UpsertTag(tagValue1);
            sut.UpsertTag(tagValue2);
            sut.UpsertTag(tagValue3);
            var result = sut.GetTagValues("ab");

            Assert.DoesNotContain(result, a => a == tagValue1);
            Assert.Single(result, a => a == tagValue2);
            Assert.Single(result, a => a == tagValue3);
        }

        [Fact]
        public void GetAllTagValues()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var tagValue1 = "aa";
            var tagValue2 = "ab";
            var tagValue3 = "abc";
            var sut = fixture.Create<Org>();

            sut.UpsertTag(tagValue1);
            sut.UpsertTag(tagValue2);
            sut.UpsertTag(tagValue3);
            var result = sut.GetTagValues();

            Assert.Equal(3, result.Count());
        }

        [Fact]
        public void FindStudentLimit()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var sut = fixture.Create<Org>();
            var config = new Mock<IVkConfig>();

            config.Setup(x => x.DefaultStudentLimit).Returns(10);
            sut.StudentLimit = 11;
            var result = sut.GetStudentLimit(config.Object);
            Assert.Equal(11, result);


            config.Setup(x => x.DefaultStudentLimit).Returns(15);
            sut.StudentLimit = 10;
            result = sut.GetStudentLimit(config.Object);
            Assert.Equal(15, result);
        }

        [Fact]
        public void AllowInvitesWhenBelowStudentLimit()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var sut = fixture.Create<Org>();
            sut.StudentLimit = 0;

            // limit = 3
            var config = new Mock<IVkConfig>();
            config.Setup(x => x.DefaultStudentLimit).Returns(3);

            // existing potential students = 2
            sut.AddStudentInvite("test@test.com");
            sut.AddStudent(fixture.Create<VkUser>());

            var result = sut.CanAddStudents(1, config.Object);

            Assert.True(result);
        }

        [Fact]
        public void BlockInvitesWhenAtStudentLimit()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var sut = fixture.Create<Org>();
            sut.StudentLimit = 0;

            // limit = 2
            var config = new Mock<IVkConfig>();
            config.Setup(x => x.DefaultStudentLimit).Returns(2);

            // existing potential students = 2
            sut.AddStudentInvite("test1@test.com");
            sut.AddStudent(fixture.Create<VkUser>());

            var result = sut.CanAddStudents(1, config.Object);

            Assert.False(result);
        }

        [Fact]
        public void BlockInvitesWhenAboveStudentLimit()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var sut = fixture.Create<Org>();
            sut.StudentLimit = 0;

            // limit = 2
            var config = new Mock<IVkConfig>();
            config.Setup(x => x.DefaultStudentLimit).Returns(2);

            // existing potential students = 3
            sut.AddStudentInvite("test1@test.com");
            sut.AddStudent(fixture.Create<VkUser>());

            var result = sut.CanAddStudents(1, config.Object);

            Assert.False(result);
        }
    }
}
