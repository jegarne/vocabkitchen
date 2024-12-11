namespace VkCore.Dtos
{
    public class WordAttemptSummaryDto
   {
        public string WordEntryId { get; set; }
        public string Word { get; set; }
        public int SpellingFailures { get; set; }
        public int SpellingAttempts { get; set; }
        public int MeaningFailures { get; set; }
        public int MeaningAttempts { get; set; }
        public int ClozeFailures { get; set; }
        public int ClozeAttempts { get; set; }
    }
}
