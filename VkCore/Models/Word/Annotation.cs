using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using VkCore.Constants;
using VkCore.Interfaces;
using VkCore.SharedKernel;

namespace VkCore.Models.Word
{
    public enum AnnotationType
    {
        Definition,
        Translation
    }

    public class Annotation : BaseEntity, IUserUpdateable
    {
        private HashSet<AnnotationContext> _annotationContexts;

        private Annotation() { }
        public Annotation(string wordEntryId)
        {
            Id = Guid.NewGuid().ToString();
            WordEntryId = wordEntryId;
            _annotationContexts = new HashSet<AnnotationContext>();
        }
       
        public void AddContext(string context, string readingId = null, DbContext dbContext = null)
        {
            if (_annotationContexts == null)
            {
                if (dbContext == null)
                {
                    throw new ArgumentNullException(nameof(AnnotationContexts),
                        $"You must provide a context if the AnnotationContexts collection isn't valid.");
                }
                dbContext.Entry(this).Collection(o => o.AnnotationContexts).Load();
            }

            if(_annotationContexts.Any(c => c.Value == context && c.ReadingId == readingId)) return;
            
            _annotationContexts.Add(new AnnotationContext(this.Id, context, readingId));
        }

        public bool IsUserCreated()
        {
            return Source == DefinitionSourceTypes.UserCode;
        }

        public void SetUpdatedBy(string userId)
        {
            this.UpdatedBy = userId;
            this.UpdateDate = DateTime.UtcNow;
        }

        public string WordEntryId { get; set; }
        public string PartOfSpeech { get; set; }
        public string Source { get; set; }
        public string ImageUrl { get; set; }
        public IEnumerable<StudentWord> Students { get; set; }
        public AnnotationType Type { get; set; }
        public string Value { get; set; }
        public IEnumerable<AnnotationContext> AnnotationContexts => _annotationContexts?.ToList();

        public string UpdatedBy { get; set;  }
        public DateTime UpdateDate { get; set; }
    }
}
