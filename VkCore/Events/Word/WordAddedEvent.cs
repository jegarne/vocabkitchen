using MediatR;

namespace VkCore.Events.Word
{
    public class WordAddedEvent : INotification
    {
        public WordAddedEvent(string word)
        {
            Word = word;
        }

        public string Word { get; set; }
    }
}
