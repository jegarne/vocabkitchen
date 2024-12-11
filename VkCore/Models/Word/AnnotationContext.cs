using System;
using VkCore.SharedKernel;

namespace VkCore.Models.Word
{
    public class AnnotationContext : BaseEntity
    {
        public AnnotationContext(string annotationId, string value, string readingId = null, string contentItemId = null)
        {
            Id = Guid.NewGuid().ToString();
            AnnotationId = annotationId;
            Value = value;
            ReadingId = readingId;
            ContentItemId = contentItemId;
        }
        public string Value { get; set; }

        public string AnnotationId { get; set; }
        public string ReadingId { get; set; }
        public string ContentItemId { get; set; }
    }
}
