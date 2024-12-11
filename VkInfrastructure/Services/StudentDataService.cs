using System;
using System.Linq;
using VkCore.Interfaces;
using VkInfrastructure.Data;

namespace VkInfrastructure.Services
{
    public class StudentDataService : IStudentDataService
    {
        private readonly VkDbContext _context;
        private readonly string _userId;

        public StudentDataService(VkDbContext context, string userId)
        {
            _context = context;
            _userId = userId;
        }

        public int GetKnownWordsCount()
        {
            return _context.StudentWords.Count(w => w.VkUserId == _userId && w.IsKnown == true);
        }

        public int GetInProgressWordsCount()
        {
            return _context.StudentWords.Count(w => w.VkUserId == _userId && !w.IsKnown);
        }

        public int GetNewReadingsCount()
        {
            var studentWordIds = _context.StudentWords.Where(w => w.VkUserId == _userId).Select(w => w.WordEntryId);
            return _context.Readings.Where(r => r.Tags.Any(t => t.Tag.Users.Any(u => u.UserId == _userId)))
                .SelectMany(r => r.ContentItems).Where(c => c.WordId != null && !studentWordIds.Contains(c.WordId))
                .Select(c => c.ReadingId).Distinct().Count();
        }

        public int GetNewWordsCount()
        {
            var studentWordIds = _context.StudentWords.Where(w => w.VkUserId == _userId).Select(w => w.WordEntryId);
            return _context.Readings.Where(r => r.Tags.Any(t => t.Tag.Users.Any(u => u.UserId == _userId)))
                .SelectMany(r => r.ContentItems).Where(c => c.WordId != null && !studentWordIds.Contains(c.WordId))
                .Select(c => c.Id).Distinct().Count();
        }
    }
}
