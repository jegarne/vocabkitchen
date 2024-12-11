using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using VkCore.SharedKernel;

namespace VkCore.Models.TagModel
{
    public class Tag : BaseEntity
    {
        private HashSet<ReadingTag> _readings;
        private HashSet<UserTag> _users;

        private Tag() { }
        public Tag(string orgId, string value)
        {
            Id = Guid.NewGuid().ToString();
            OrgId = orgId;
            Value = value;
            _readings = new HashSet<ReadingTag>();
            _users = new HashSet<UserTag>();
        }

        public string OrgId { get; set; }
        public string Value { get; set; }
        public bool IsDefault { get; set; }

        public IEnumerable<ReadingTag> Readings => _readings?.ToList();
        public IEnumerable<UserTag> Users => _users?.ToList();

        public void AddReading(string readingId, DbContext context = null)
        {
            if (_readings == null)
            {
                NullCheckContext(context, "readings");
                context.Entry(this).Collection(o => o.Readings).Load();
            }

            _readings.Add(new ReadingTag(readingId, this));
        }

        public void MergeReadings(IEnumerable<string> readingIds, DbContext context = null)
        {
            if (_readings == null)
            {
                NullCheckContext(context, "readings");
                context.Entry(this).Collection(o => o.Readings).Load();
            }
            _readings.RemoveWhere(r => readingIds.All(y => y != r.ReadingId));

            var idsToAdd = readingIds.Where(id => _readings.All(r => r.ReadingId != id));

            foreach (var id in idsToAdd)
            {
                _readings.Add(new ReadingTag(id, this));
            }
        }

        public void AddUser(string studentId, DbContext context = null)
        {
            if (_users == null)
            {
                NullCheckContext(context, "students");
                context.Entry(this).Collection(o => o.Users).Load();
            }

            _users.Add(new UserTag(studentId, this.Id));
        }

        public void RemoveUser(string studentId, DbContext context = null)
        {
            if (_users == null)
            {
                NullCheckContext(context, "students");
                context.Entry(this).Collection(o => o.Users).Load();
            }

            _users.RemoveWhere(s => s.UserId == studentId);
        }

        public void MergeUsers(IEnumerable<string> userIds, DbContext context = null)
        {
            if (_users == null)
            {
                NullCheckContext(context, "students");
                context.Entry(this).Collection(o => o.Users).Load();
            }
            _users.RemoveWhere(r => userIds.All(y => y != r.UserId));

            var idsToAdd = userIds.Where(id => _users.All(r => r.UserId != id));

            foreach (var id in idsToAdd)
            {
                _users.Add(new UserTag(id, this.Id));
            }
        }

        private void NullCheckContext(DbContext context, string collection)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context),
                    $"You must provide a context if the {collection} collection isn't valid.");
            }
        }
    }
}
