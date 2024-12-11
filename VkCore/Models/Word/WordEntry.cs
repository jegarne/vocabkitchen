using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using VkCore.Events.Word;
using VkCore.Extensions;
using VkCore.Interfaces;
using VkCore.SharedKernel;

namespace VkCore.Models.Word
{
    public class WordEntry : BaseEntity, IResponseErrorLogger
    {
        // Use uninitialised backing fields - this means we can detect if the collection was loaded
        // don't listen to resharper, these should not be readonly as then EF can't write to them
        private HashSet<Annotation> _annotations;
        private List<string> _errors = new List<string>();

        public string Word { get; set; }
        public IEnumerable<Annotation> Annotations => _annotations?.ToList();
        public IEnumerable<StudentWord> Students { get; set; }

        private WordEntry() { }

        public WordEntry(string word)
        {
            Id = Guid.NewGuid().ToString();
            var cleanWord = word.RemoveNonAlphaNumeric();
            Word = cleanWord;
            _annotations = new HashSet<Annotation>();
            AddEvent(new WordAddedEvent(cleanWord));
        }

        public void AddAnnotation(Annotation newAnnotation, DbContext dbContext = null
        ) {
            if (_annotations == null)
            {
                if (dbContext == null)
                {
                    throw new ArgumentNullException(nameof(dbContext),
                        $"You must provide a context if the annotation collection isn't valid.");
                }
                dbContext.Entry(this).Collection(o => o.Annotations).Load();
            }

            if (newAnnotation.IsUserCreated()  && string.IsNullOrEmpty(newAnnotation.UpdatedBy))
            {
                throw new ArgumentException("If an annotation is user created, the UpdatedBy property must be set.");
            }

            _annotations.Add(newAnnotation);
        }

        public IEnumerable<string> Errors { get; }
    }
}
