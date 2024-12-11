using VkCore.Constants;
using VkCore.Models.Word;
using Xunit;

namespace VkCore.Test.Models
{
    public class StudentWordShould
    {
        [Fact]
        public void NotSetIsKnownWithOnlyMeaningAttemptTypes()
        {
            var sut = new StudentWord("1", "1", "1");

            sut.AddAttempt(WordAttemptTypes.Meaning, true);
            sut.AddAttempt(WordAttemptTypes.Meaning, true);
            sut.AddAttempt(WordAttemptTypes.Meaning, true);

            Assert.False(sut.IsKnown);
        }

        [Fact]
        public void NotSetIsKnownWithOnlySpellingAttemptTypes()
        {
            var sut = new StudentWord("1", "1", "1");

            sut.AddAttempt(WordAttemptTypes.Spelling, true);
            sut.AddAttempt(WordAttemptTypes.Spelling, true);
            sut.AddAttempt(WordAttemptTypes.Spelling, true);

            Assert.False(sut.IsKnown);
        }

        [Fact]
        public void NotSetIsKnownWithOnlyClozeAttemptTypes()
        {
            var sut = new StudentWord("1", "1", "1");

            sut.AddAttempt(WordAttemptTypes.Cloze, true);
            sut.AddAttempt(WordAttemptTypes.Cloze, true);
            sut.AddAttempt(WordAttemptTypes.Cloze, true);

            Assert.False(sut.IsKnown);
        }

        [Fact]
        public void NotSetIsKnownWithOnlyOneOfEachAttemptType()
        {
            var sut = new StudentWord("1", "1", "1");

            sut.AddAttempt(WordAttemptTypes.Spelling, true);
            sut.AddAttempt(WordAttemptTypes.Cloze, true);
            sut.AddAttempt(WordAttemptTypes.Meaning, true);

            Assert.False(sut.IsKnown);
        }

        [Fact]
        public void SetIsKnownWithTwoOfEachAttemptType()
        {
            var sut = new StudentWord("1", "1", "1");

            sut.AddAttempt(WordAttemptTypes.Spelling, true);
            sut.AddAttempt(WordAttemptTypes.Cloze, true);
            sut.AddAttempt(WordAttemptTypes.Meaning, true);

            sut.AddAttempt(WordAttemptTypes.Spelling, true);
            sut.AddAttempt(WordAttemptTypes.Cloze, true);
            sut.AddAttempt(WordAttemptTypes.Meaning, true);

            Assert.True(sut.IsKnown);
        }

        [Fact]
        public void UpdateIsKnownDateAfterAReview()
        {
            var sut = new StudentWord("1", "1", "1");

            sut.AddAttempt(WordAttemptTypes.Spelling, true);
            sut.AddAttempt(WordAttemptTypes.Cloze, true);
            sut.AddAttempt(WordAttemptTypes.Meaning, true);

            sut.AddAttempt(WordAttemptTypes.Spelling, true);
            sut.AddAttempt(WordAttemptTypes.Cloze, true);
            sut.AddAttempt(WordAttemptTypes.Meaning, true);

            var isKnownDate1 = sut.IsKnownDate;

            sut.AddAttempt(WordAttemptTypes.Spelling, true);
            sut.AddAttempt(WordAttemptTypes.Cloze, true);

            Assert.True(sut.IsKnownDate == isKnownDate1);

            sut.AddAttempt(WordAttemptTypes.Meaning, true);

            Assert.True(sut.IsKnownDate != isKnownDate1);
        }
    }
}
