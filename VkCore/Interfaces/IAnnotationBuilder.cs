using VkCore.Models.Word;


namespace VkCore.Interfaces
{
    public interface IAnnotationBuilder
    {
        void SetContent(string definition, string partOfSpeech, string imageUrl = null);
        void AddExampleSentence(string exampleSentence, string readingId);
        void SetSource(string source, string createUserId);
        Annotation GetAnnotation();
    }
}
