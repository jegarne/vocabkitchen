using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VkCore.Constants;
using VkCore.SharedKernel;

namespace VkCore.Models.Word
{
    public class StudentWord : BaseEntity
    {
        private HashSet<WordAttempt> _attempts;

        private StudentWord() { }
        public StudentWord(string vkUserId, string wordEntryId, string annotationId)
        {
            Id = Guid.NewGuid().ToString();
            VkUserId = vkUserId;
            WordEntryId = wordEntryId;
            AnnotationId = annotationId;
            CreateDate = DateTime.UtcNow;
            _attempts = new HashSet<WordAttempt>();
        }

        public bool IsKnown { get; private set; }
        public DateTime IsKnownDate { get; private set; }
        public DateTime CreateDate { get; }

        public IEnumerable<WordAttempt> Attempts => _attempts?.ToList();
        public IEnumerable<WordAttempt> ClozeAttempts => _attempts?
            .Where(a => a.AttemptType == WordAttemptTypes.Cloze).ToList();
        public IEnumerable<WordAttempt> MeaningAttempts => _attempts?
            .Where(a => a.AttemptType == WordAttemptTypes.Meaning).ToList();
        public IEnumerable<WordAttempt> SpellingAttempts => _attempts?
            .Where(a => a.AttemptType == WordAttemptTypes.Spelling).ToList();

        public void AddAttempt(string type, bool wasSuccessful, DbContext context = null)
        {
            if (_attempts == null)
            {
                NullCheckContext(context, "attempts");
                context.Entry(this).Collection(o => o.Attempts).Load();
            }

            _attempts.Add(new WordAttempt(type, wasSuccessful, Id));
                       
            if (this.IsKnown)
            {
                // update the learned date if successfully reviewed
                var successfulAttempts = _attempts.Where(x => x.AttemptDate > this.IsKnownDate && x.WasSuccessful == true).ToList();
                if (successfulAttempts.Any(x => x.AttemptType == WordAttemptTypes.Cloze)
                    && successfulAttempts.Any(x => x.AttemptType == WordAttemptTypes.Spelling)
                    && successfulAttempts.Any(x => x.AttemptType == WordAttemptTypes.Meaning)
                ) {
                    this.MarkAsKnown();
                }

                return;
            }

            var meaning = _attempts.Count(a => a.AttemptType == WordAttemptTypes.Meaning && a.WasSuccessful);
            var spelling = _attempts.Count(a => a.AttemptType == WordAttemptTypes.Spelling && a.WasSuccessful);
            var cloze = _attempts.Count(a => a.AttemptType == WordAttemptTypes.Spelling && a.WasSuccessful);

            // logic for marking words known based on a count
            // TODO: set this in the teacher's dashboard
            if (meaning > 1 && spelling > 1 && cloze > 1)
            {
                this.MarkAsKnown();
            }
        }

        public void MarkAsKnown()
        {
            IsKnown = true;
            IsKnownDate = DateTime.UtcNow;
        }

        public async Task Hydrate(DbContext context)
        {
            await context.Entry(this)
                .Reference(e => e.Annotation)
                .Query().Include(a => a.AnnotationContexts).LoadAsync();
            await context.Entry(this).Reference(e => e.WordEntry).LoadAsync();
            await context.Entry(this).Collection(e => e.Attempts).LoadAsync();
        }

        public string VkUserId { get; set; }
        public VkUser User { get; set; }

        public string WordEntryId { get; set; }
        public WordEntry WordEntry { get; set; }

        public string AnnotationId { get; set; }
        public Annotation Annotation { get; set; }

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
