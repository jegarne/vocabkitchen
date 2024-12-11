using System;
using VkCore.SharedKernel;

namespace VkCore.Models.Word
{
    public class WordAttempt : BaseEntity
    {
        private WordAttempt() { }
        public WordAttempt(string attemptType, bool wasSuccessful, string studentWordId)
        {
            Id = Guid.NewGuid().ToString();
            AttemptType = attemptType;
            WasSuccessful = wasSuccessful;
            StudentWordId = studentWordId;
            AttemptDate = DateTime.UtcNow;
        }

        public DateTime AttemptDate { get; set; }
        public string AttemptType { get; set; }
        public bool WasSuccessful { get; set; }

        public string StudentWordId { get; set; }
        public StudentWord Word { get; set; }
    }
}
