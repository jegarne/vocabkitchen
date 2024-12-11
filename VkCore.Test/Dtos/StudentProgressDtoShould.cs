using System.Linq;
using VkCore.Constants;
using VkCore.Dtos;
using VkCore.Models.Word;
using Xunit;

namespace VkCore.Test.Dtos
{
    public class StudentProgressDtoShould
    {
        [Fact]
        public void CalculateFailurePercentages()
        {

            var studentWord = new StudentWord("userId", "wordEntryId", "annotationId")
            {
                WordEntry = new WordEntry("test")
            };
            studentWord.AddAttempt(WordAttemptTypes.Cloze, false);
            studentWord.AddAttempt(WordAttemptTypes.Cloze, true);
            studentWord.AddAttempt(WordAttemptTypes.Cloze, false);

            var sut = new StudentProgressDto(new VkCore.Models.VkUser("test@test.com", "Joe", "Test"));
            sut.AddWord(studentWord);

            Assert.Equal(2, sut.Words.First().ClozeFailedAttempts);

        }
    }
}
