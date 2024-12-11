using System.Linq;
using Xunit;
using VkCore.Models.Profiler;

namespace VkCore.Test.Models.Profiler
{
    public class PunctuationTokenizerShould
    {
        [Fact]
        public void SplitTextAndKeepPunctuation()
        {
            var text = System.Environment.NewLine + " . , \" : ' ? ! % -";

            var sut = new PunctuationTokenizer();
            var result = sut.Tokenize(text);

            Assert.Equal(10, result.Length);        
        }

        [Fact]
        public void IdentifyLeftAndRightDoubleQuote()
        {
            var text = "test \"test\" test";

            var sut = new PunctuationTokenizer();
            var result = sut.Tokenize(text);


            var actual = string.Join(" ", result);
            var expected = "test 00leftdoublequote00 test 00rightdoublequote00 test";
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void IdentifyLeftAndRightSingleQuote()
        {
            var text = "test 'test' test's";

            var sut = new PunctuationTokenizer();
            var result = sut.Tokenize(text);


            var actual = string.Join(" ", result);
            var expected = "test 00leftsinglequote00 test 00rightsinglequote00 test 00apostrophe00 s";
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ReturnCorrectPunctuation()
        {
            var text = System.Environment.NewLine + " . , \" : ' ? ! % - word — ’";

            var sut = new PunctuationTokenizer();
            var result = sut.Tokenize(text);

            Assert.Equal("<br />", sut.ConvertToHtmlEntity(result[0]));
            Assert.Equal(". ", sut.ConvertToHtmlEntity(result[1]));
            Assert.Equal(", ", sut.ConvertToHtmlEntity(result[2]));
            Assert.Equal("&quot; ", sut.ConvertToHtmlEntity(result[3]));
            Assert.Equal("&#58; ", sut.ConvertToHtmlEntity(result[4]));
            Assert.Equal(" &#39;", sut.ConvertToHtmlEntity(result[5]));
            Assert.Equal("? ", sut.ConvertToHtmlEntity(result[6]));
            Assert.Equal("! ", sut.ConvertToHtmlEntity(result[7]));
            Assert.Equal("% ", sut.ConvertToHtmlEntity(result[8]));
            Assert.Equal("-", sut.ConvertToHtmlEntity(result[9]));
            Assert.Equal("", sut.ConvertToHtmlEntity(result[10]));
            Assert.Equal(" — ", sut.ConvertToHtmlEntity(result[11]));
            Assert.Equal("’", sut.ConvertToHtmlEntity(result[12]));
        }
    }
}
