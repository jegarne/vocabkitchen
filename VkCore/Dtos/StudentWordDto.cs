using System;
using System.Collections.Generic;
using System.Linq;
using VkCore.Models.Word;

namespace VkCore.Dtos
{
    public class StudentWordDto
    {
        public StudentWordDto(string id, string annotationId, string word, string definition, IEnumerable<string> sentences)
        {
            Id = id;
            AnnotationId = annotationId;
            Word = word;
            Definition = definition;
            Sentences = sentences;
        }

        public StudentWordDto(StudentWord word)
        {
            Id = word.WordEntryId;
            AnnotationId = word.AnnotationId;
            IsKnown = word.IsKnown;
            Word = word.WordEntry.Word;
            Definition = word.Annotation.Value;
            Sentences = word.Annotation.AnnotationContexts?.Select(c => c.Value);
            Attempts = word.Attempts?.Select(a => new WordAttemptDto(a));
        }

        public string Id { get; set; }
        public string AnnotationId { get; set; }
        public bool IsKnown { get; set; }
        public string Word { get; set; }
        public string Definition { get; set; }
        public IEnumerable<string> Sentences { get; set; }
        public IEnumerable<WordAttemptDto> Attempts { get; set; }
    }

    public class WordAttemptDto
    {
        public WordAttemptDto(WordAttempt attempt)
        {
            AttemptDate = attempt.AttemptDate;
            AttemptType = attempt.AttemptType;
            WasSuccessful = attempt.WasSuccessful;
        }

        public DateTime AttemptDate { get; set; }
        public string AttemptType { get; set; }
        public bool WasSuccessful { get; set; }
    }
}
