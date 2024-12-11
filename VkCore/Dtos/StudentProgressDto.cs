using System;
using System.Collections.Generic;
using System.Linq;
using VkCore.Models;
using VkCore.Models.Word;

namespace VkCore.Dtos
{
    public class StudentProgressDto : StudentDto
    {
        private List<WordProgressDto> _words = new List<WordProgressDto>();

        public StudentProgressDto(VkUser user, IEnumerable<TagDto> tags = null) : base(user, tags)
        {
        }

        public IEnumerable<WordProgressDto> Words => _words;

        public void AddWord(StudentWord w)
        {
            var dto = new WordProgressDto();
            dto.Id = w.Id;
            dto.Word = w.WordEntry.Word;

            if (w.IsKnown)
            {
                dto.IsKnown = true;
                dto.IsKnownDate = w.IsKnownDate;
            }
            else if (w.Attempts != null && w.Attempts.Any())
                dto.IsInProgress = true;
            else
                dto.IsNew = true;

            dto.TotalAttempts = w.Attempts?.Count() ?? 0;

            if (dto.TotalAttempts == 0)

            {
                _words.Add(dto);
                return;
            }

            dto.ClozeFailedAttempts = w.ClozeAttempts.Count(a => !a.WasSuccessful);
            dto.SpellingFailedAttempts = w.SpellingAttempts.Count(a => !a.WasSuccessful);
            dto.MeaningFailedAttempts = w.MeaningAttempts.Count(a => !a.WasSuccessful);

            _words.Add(dto);
        }
    }

    public class WordProgressDto
    {
        public string Id { get; set; }
        public string AnnotationId { get; set; }
        public string Word { get; set; }
        public bool IsKnown { get; set; }
        public bool IsInProgress { get; set; }
        public bool IsNew { get; set; }
        public DateTime IsKnownDate { get; set; }

        public int TotalAttempts { get; set; }
        public decimal SpellingFailedAttempts { get; set; }
        public decimal ClozeFailedAttempts { get; set; }
        public decimal MeaningFailedAttempts { get; set; }

        public string Status
        {
            get
            {
                if (this.IsNew) return "new";
                if (this.IsInProgress) return "in progress";
                if (this.IsKnown) return "known";
                return "";
            }
        }

    }
}
