using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VkCore.Services;
using Xunit;

namespace VkCore.Test.Services
{
    public class ExampleSentenceServiceShould
    {
        [Fact]
        public void IndexSentencesAtTheCorrectLength()
        {
            var text = "A sentence with a defined word that you can see." +
                       "\n\nWhen developing specifics for Little Fox, the couple reflected back on" +
                       "their time in Brooklyn. While at Clover Club, which is owned by cocktail expert" +
                       " Julie Reiner and hospitality pro Susan Fedroff, the Rivards met mixologist Shannon Ponche, " +
                       "who's now developing Little Fox's cocktail list. Like Craig, Ponche is a St. Louis native, having " +
                       "worked with cocktail guru Ted Kilgore at Taste years ago before moving to New York. In general, The Rivards " +
                       "say that they gravitate toward driver, herbal-forward cocktails and those made with fortified wines like " +
                       "sherry and vermouth, so diners can expect to see those influences on the menu at Little Fox.\n\nAbout the one thing.";


            var sut = new ExampleSentenceService(text);

            var maxIndex = sut.SentenceIndexes.Max(x => x.Value);
            var sentences = sut.Sentences;

            Assert.Equal(text.Length, maxIndex);
        }

        [Fact]
        public void NotBlowUpWhenATextDoesNotHaveAPeriod()
        {
            var text = "A sentence with a defined word that you can see" +
                       "\n\nWhen developing specifics for Little Fox, the couple reflected back on" +
                       "their time in Brooklyn while at Clover Club, which is owned by cocktail expert";            

            var sut = new ExampleSentenceService(text);

            var maxIndex = sut.SentenceIndexes.Max(x => x.Value);
            var sentences = sut.Sentences;

            Assert.Equal(text.Length, maxIndex);
        }

        [Fact]
        public void IndexSentencesAtTheCorrectLength2()
        {
                                      //17              33                              61
            var text = "    One sentence.  Two sentences.  \n\n\n\nThree sentences.      ";

            var sut = new ExampleSentenceService(text);

            var maxIndex = sut.SentenceIndexes.Max(x => x.Value);
            var sentences = sut.Sentences;

            Assert.Equal(text.Length, maxIndex);
            Assert.Equal(3, sentences.Count);

            Assert.Equal(17, sut.SentenceIndexes[0]);
            Assert.Equal(33, sut.SentenceIndexes[1]);
            Assert.Equal(61, sut.SentenceIndexes[2]);
        }
    }
}
