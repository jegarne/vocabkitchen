using VkCore.Models.ReadingModel;

namespace VkCore.Dtos
{
    public class ContentItemDto
    {
        public ContentItemDto(
            string id,
            int index, 
            string value, 
            string definition = null, 
            string wordId = null, 
            string annotationId = null, 
            string annotationContextId = null
        )
        {
            Id = id;
            Index = index;
            Value = value;
            Definition = definition;
            WordId = wordId;
            AnnotationId = annotationId;
            AnnotationContextId = annotationContextId;
        }

        public string Id { get; set; }
        public int Index { get; set; }
        public string Value { get; set; }
        public string Definition { get; set; }

        public string WordId { get; set; }
        public string AnnotationId { get; set; }
        public string AnnotationContextId { get; set; }

        public int DefinitionUsedByWordsCount { get; set; }
        public int DefinitionUsedByStudentsCount { get; set; }
    }
}
