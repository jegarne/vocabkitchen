using System;
using VkCore.Constants;
using VkCore.Interfaces;
using VkCore.Models.Word;

namespace VkCore.Builders
{
    public class DefinitionBuilder : IAnnotationBuilder
    {
        private readonly Annotation _annotation;

        public DefinitionBuilder(string wordEntryId)
        {
            _annotation = new Annotation(wordEntryId);
        }

        public void SetContent(string definition, string partOfSpeech, string imageUrl = null)
        {
            _annotation.Type = AnnotationType.Definition;
            _annotation.Value = definition;
            _annotation.PartOfSpeech = partOfSpeech;
            _annotation.ImageUrl = imageUrl;
        }

        public void AddExampleSentence(string exampleSentence, string readingId)
        {
            _annotation.AddContext(exampleSentence, readingId);
        }

        public void SetSource(string source, string createUserId)
        {
            if (source == DefinitionSourceTypes.UserCode)
            {
                _annotation.UpdatedBy = createUserId;
                _annotation.UpdateDate = DateTime.UtcNow;
            }

            _annotation.Source = source;
        }

        public Annotation GetAnnotation()
        {
            return _annotation;
        }
    }
}
