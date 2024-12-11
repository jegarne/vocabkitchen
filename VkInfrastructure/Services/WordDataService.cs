using System;
using VkCore.Interfaces;
using VkInfrastructure.Data;
using System.Linq;

namespace VkInfrastructure.Services
{
    public class WordDataService
    {
        private readonly VkDbContext _context;
        private readonly string _wordEntryId;

        public WordDataService(VkDbContext context, string wordEntryId)
        {
            _context = context;
            _wordEntryId = wordEntryId;
        }

        public int SpellingFailures()
        {
            return _context.StudentWords.Where(w => w.WordEntryId == _wordEntryId)
                .SelectMany(x => x.SpellingAttempts)
                .Count(a => a.WasSuccessful == false );
        }

        public int MeaningFailures()
        {
            return _context.StudentWords.Where(w => w.WordEntryId == _wordEntryId)
                .SelectMany(x => x.MeaningAttempts)
                .Count(a => a.WasSuccessful == false);
        }

        public int ClozeFailures()
        {
            throw new NotImplementedException();
        }

        public int StudentMasteryCount()
        {
            throw new NotImplementedException();
        }
    }
}
