using System;

namespace VkCore.Dtos
{
    public class DefinitionDto
    {
        public string PartOfSpeech { get; set; }
        public string Source { get; set; }
        public string ImageUrl { get; set; }
        public string Value { get; set; }
        public string AnnotationId { get; set; }
        public string LastUpdateDate { get; set; }
        public bool IsEditable { get; set; } = true;
        public bool IsMine { get; set; } = false;
    }
}
