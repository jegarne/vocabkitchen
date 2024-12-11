using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using VkCore.Events.Organization;
using VkCore.Extensions;
using VkCore.Interfaces;
using VkCore.Models.Invite;
using VkCore.Models.TagModel;
using VkCore.SharedKernel;

namespace VkCore.Models.Organization
{
    public class Org : BaseEntity, IResponseErrorLogger
    {
        private List<string> _errors = new List<string>();
        [NotMapped]
        public IEnumerable<string> Errors => _errors;

        public string Name { get; set; }
        public int StudentLimit { get; set; }

        //-----------------------------------------------
        //relationships

        // Use uninitialised backing fields - this means we can detect if the collection was loaded
        // don't listen to resharper, these should not be readonly as then EF can't write to them
        private HashSet<OrgAdmin> _admins;
        private HashSet<OrgTeacher> _teachers;
        private HashSet<OrgStudent> _students;
        private HashSet<OrgReading> _readings;
        private HashSet<Tag> _tags;
        private HashSet<OrgInvite> _invites;

        public IEnumerable<OrgAdmin> Admins => _admins?.ToList();
        public IEnumerable<OrgTeacher> Teachers => _teachers?.ToList();
        public IEnumerable<OrgStudent> Students => _students?.ToList();
        public IEnumerable<OrgReading> Readings => _readings?.ToList();
        public IEnumerable<OrgInvite> Invites => _invites?.ToList();
        public IEnumerable<Tag> Tags => _tags?.ToList();

        private Org() { }

        public Org(string name, int studentLimit)
        {
            Id = Guid.NewGuid().ToString();
            Name = name;
            StudentLimit = studentLimit;
            _admins = new HashSet<OrgAdmin>();
            _teachers = new HashSet<OrgTeacher>();
            _students = new HashSet<OrgStudent>();
            _readings = new HashSet<OrgReading>();
            _tags = new HashSet<Tag>();
            _invites = new HashSet<OrgInvite>();
        }

        #region Invites Crud

        public void AddTeacherInvite(string email, DbContext context = null)
        {
            if (_invites == null)
            {
                NullCheckContext(context, "invites");
                context.Entry(this).Collection(o => o.Invites).Load();
            }

            if (_invites.Any(t => t.Email == email && t.IsTeacherInvite()))
            {
                _errors.Add($"{email} was already invited to this organization.");
                return;
            }

            if (_teachers == null)
            {
                context.Entry(this).Collection(x => x.Teachers).Query()
                    .Include(y => y.TeacherUser)
                    .Load();
            }

            if (_teachers != null && _teachers.Any(t => t.TeacherUser.Email == email))
            {
                _errors.Add($"{email} is already a teacher in this organization.");
                return;
            }

            var invite = new OrgInvite(this.Id, email, OrgInviteType.Teacher);
            _invites.Add(invite);
        }

        public IEnumerable<OrgInvite> GetTeacherInvites(DbContext context = null)
        {
            if (_invites == null)
            {
                NullCheckContext(context, "invites");
                context.Entry(this).Collection(o => o.Invites).Load();
            }

            return _invites?.Where(i => i.IsTeacherInvite()).ToList();
        }

        public void AcceptTeacherInvite(VkUser user, DbContext context)
        {
            if (_invites == null)
            {
                NullCheckContext(context, "invites");
                context.Entry(this).Collection(o => o.Invites).Load();
            }

            var invite = GetTeacherInvites(context).FirstOrDefault(x => x.Email == user.Email);

            if (invite == null)
            {
                _errors.Add($"{user.Email} has not been invited to this organization.");
                return;
            }

            AddTeacher(user, context);

            if (_teachers.Any(x => x.VkUserId == user.Id))
                _invites.RemoveWhere(x => x.Email == user.Email && x.IsTeacherInvite());
        }

        public int GetStudentLimit(IVkConfig config)
        {
            return config.DefaultStudentLimit >= StudentLimit ? config.DefaultStudentLimit : StudentLimit;
        }

        public bool CanAddStudents(int newInviteCount, IVkConfig config, DbContext context = null)
        {
            if (_invites == null)
            {
                NullCheckContext(context, "invites");
                context.Entry(this).Collection(o => o.Invites).Load();
            }

            if (_students == null)
            {
                context.Entry(this).Collection(x => x.Students).Query()
                    .Include(y => y.StudentUser)
                    .Load();
            }

            var studentLimit = GetStudentLimit(config);
            var potentialStudentCount = _students?.Count() + _invites?.Count();
            var proposedPotentialStudentCount = _students?.Count() + _invites?.Count() + newInviteCount;
            if (proposedPotentialStudentCount > studentLimit)
            {
                var newInviteStudents = "student".Pluralize(newInviteCount, "students");
                var potentialStudents = "student".Pluralize(potentialStudentCount ?? 0, "students");
                var limitedStudents = "student".Pluralize(studentLimit, "students");
                _errors.Add($"You're attempting to add {newInviteCount} new {newInviteStudents}. You already have {potentialStudentCount} potential {potentialStudents}." +
                            $" Your limit is {studentLimit} {limitedStudents}.  Please remove students or student invites before adding more. "
                            + "Email contact@vocabkitchen.com if you'd like to request an increase in your student limit.");
                return false;
            }

            return true;
        }

        public void AddStudentInvite(string email, DbContext context = null)
        {
            if (_invites == null)
            {
                NullCheckContext(context, "invites");
                context.Entry(this).Collection(o => o.Invites).Load();
            }

            if (_invites.Any(t => t.Email == email && t.IsStudentInvite()))
            {
                _errors.Add($"{email} was already invited to this organization.");
                return;
            }

            if (_students == null)
            {
                context.Entry(this).Collection(x => x.Students).Query()
                    .Include(y => y.StudentUser)
                    .Load();
            }

            if (_students != null && _students.Any(t => t.StudentUser.Email == email))
            {
                _errors.Add($"{email} is already a student in this organization.");
                return;
            }

            var invite = new OrgInvite(this.Id, email, OrgInviteType.Student);
            _invites.Add(invite);
        }

        public IEnumerable<OrgInvite> GetStudentInvites(DbContext context = null)
        {
            if (_invites == null)
            {
                NullCheckContext(context, "invites");
                context.Entry(this).Collection(o => o.Invites).Load();
            }

            return _invites?.Where(i => i.IsStudentInvite()).ToList();
        }

        public void AcceptStudentInvite(VkUser user, DbContext context)
        {
            if (_invites == null)
            {
                NullCheckContext(context, "invites");
                context.Entry(this).Collection(o => o.Invites).Load();
            }

            var invite = GetStudentInvites(context).FirstOrDefault(x => x.Email == user.Email);

            if (invite == null)
            {
                _errors.Add($"{user.Email} has not been invited to this organization.");
                return;
            }

            AddStudent(user, context);

            if (_students.Any(x => x.VkUserId == user.Id))
                _invites.RemoveWhere(x => x.Email == user.Email && x.IsStudentInvite());
        }

        public void RemoveInvite(string email, DbContext context = null)
        {
            if (_invites == null)
            {
                NullCheckContext(context, "invites");
                context.Entry(this).Collection(o => o.Invites).Load();
            }

            _invites.RemoveWhere(x => x.Email == email);
        }

        #endregion

        #region User CRUD

        public void AddAdmin(VkUser user, DbContext context = null)
        {
            if (_admins == null)
            {
                NullCheckContext(context, "admins");
                context.Entry(this).Collection(o => o.Admins).Load();
            }

            if (_admins.Any(x => x.VkUserId == user.Id))
            {
                _errors.Add($"{user.Email} is already an admin in this organization.");
                return;
            }

            var orgAdmin = new OrgAdmin(this, user);
            _admins.Add(orgAdmin);

            this.AddEvent(new OrgAdminAddedEvent(this.Id, user.Id));
        }

        public void AddTeacher(VkUser user, DbContext context = null)
        {
            if (_teachers == null)
            {
                NullCheckContext(context, "teachers");
                context.Entry(this).Collection(o => o.Teachers).Load();
            }

            if (_teachers.Any(x => x.VkUserId == user.Id))
            {
                _errors.Add($"{user.Email} is already a teacher in this organization.");
                return;
            }

            var orgTeacher = new OrgTeacher(this, user);

            orgTeacher.IsDefault = true;
            if (context != null)
            {
                var hasExistingDefaultOrg =
                    context.Set<OrgTeacher>().Any(t => t.VkUserId == user.Id && t.IsDefault == true);
                orgTeacher.IsDefault = !hasExistingDefaultOrg;
            }

            _teachers.Add(orgTeacher);

            this.AddEvent(new OrgTeacherAddedEvent(this.Id, user.Id));
        }

        public void AddStudent(VkUser user, DbContext context = null)
        {
            if (_students == null)
            {
                NullCheckContext(context, "students");
                context.Entry(this).Collection(o => o.Students).Load();
            }

            if (_students.Any(x => x.VkUserId == user.Id))
            {
                _errors.Add($"{user.Email} is already a student in this organization.");
                return;
            }

            var orgStudent = new OrgStudent(this, user);
            _students.Add(orgStudent);

            this.AddEvent(new OrgStudentAddedEvent(this.Id, user.Id));
        }

        public void RemoveAdmin(string userId, DbContext context = null)
        {
            if (_admins == null)
            {
                NullCheckContext(context, "admin");
                context.Entry(this).Collection(o => o.Admins).Load();
            }

            if (_admins.Count <= 1)
            {
                _errors.Add("An organization must have at least one admin.");
                return;
            }

            _admins.RemoveWhere(u => u.VkUserId == userId);
        }

        public void RemoveTeacher(string userId, DbContext context = null)
        {
            if (_teachers == null)
            {
                NullCheckContext(context, "teacher");
                context.Entry(this).Collection(o => o.Teachers).Load();
            }

            if (_teachers.Count <= 1)
            {
                _errors.Add("An organization must have at least one teacher.");
                return;
            }

            if (_admins == null)
            {
                context.Entry(this).Collection(o => o.Admins).Load();
            }

            if (_admins.Any(a => a.VkUserId == userId) && _admins.Count <= 1)
            {
                _errors.Add("An organization must have at least one admin.");
                return;
            }

            _admins.RemoveWhere(u => u.VkUserId == userId);
            _teachers.RemoveWhere(u => u.VkUserId == userId);
        }

        public void RemoveStudent(string userId, DbContext context = null)
        {
            if (_students == null)
            {
                NullCheckContext(context, "student");
                context.Entry(this).Collection(o => o.Students).Load();
            }

            _students.RemoveWhere(u => u.VkUserId == userId);
        }

        #endregion

        #region Reading CRUD

        public void AddReading(ReadingModel.Reading reading, DbContext context = null)
        {
            if (_readings == null)
            {
                NullCheckContext(context, "readings");
                context.Entry(this).Collection(o => o.Readings).Load();
            }

            if (string.IsNullOrWhiteSpace(reading.Title))
            {
                _errors.Add("A reading must have a title.");
                return;
            }

            var orgReading = new OrgReading(this, reading);
            _readings.Add(orgReading);
        }

        public void RemoveReading(string readingId)
        {
            _readings.RemoveWhere(r => r.ReadingId == readingId);
        }

        #endregion

        #region Tag CRUD

        public void UpsertTag(string value, DbContext context = null)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                _errors.Add("A tag cannot be empty.");
                return;
            }

            if (_tags == null)
            {
                NullCheckContext(context, "tags");
                context.Entry(this).Collection(o => o.Tags).Load();
            }

            if (_tags.Any(t => t.Value == value)) return;

            var tagEntry = new Tag(this.Id, value);
            _tags.Add(tagEntry);
        }

        public void EditTag(string oldValue, string newValue)
        {
            if (string.IsNullOrWhiteSpace(newValue))
            {
                _errors.Add("A tag cannot be empty.");
                return;
            }

            if (_tags.Any(t => t.Value == newValue))
            {
                _errors.Add("A tag with that name already exists.");
                return;
            }

            if (_tags.All(t => t.Value != oldValue))
            {
                _errors.Add("Could not find this tag.");
                return;
            }

            var tag = _tags.FirstOrDefault(t => t.Value == oldValue);
            if (tag != null) tag.Value = newValue;
        }

        public IEnumerable<string> GetTagValues(string startingValues = "", DbContext context = null)
        {
            if (_tags == null)
            {
                NullCheckContext(context, "tags");
                context.Entry(this).Collection(o => o.Tags).Load();
            }

            if (string.IsNullOrEmpty(startingValues))
                return Tags.Select(t => t.Value);

            return _tags.Select(t => t.Value).Where(v => v.StartsWith(startingValues));
        }

        public IEnumerable<Tag> SearchTags(string startingValues = "", DbContext context = null)
        {
            if (_tags == null)
            {
                NullCheckContext(context, "tags");
                context.Entry(this).Collection(o => o.Tags).Load();
            }

            if (string.IsNullOrEmpty(startingValues))
                return Tags;

            return _tags.Where(v => v.Value.StartsWith(startingValues));
        }

        #endregion

        #region Private Methods

        private void NullCheckContext(DbContext context, string collection)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context),
                    $"You must provide a context if the {collection} collection isn't valid.");
            }
        }

        #endregion


    }
}
