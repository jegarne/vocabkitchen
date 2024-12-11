using Moq;
using System.Collections.Generic;
using System.Linq;
using VkCore.Interfaces;
using VkCore.Models.Profiler;
using Xunit;

namespace VkCore.Test.Models.Profiler
{
    public class ProfileGeneratorShould
    {
        [Fact]
        public void FindWords()
        {
            var words = new string[] { "one", "two", "three", "four", "five", "six" };

            var mockListReader = new Mock<IProfilerListReader>();
            mockListReader.Setup(lr => lr.GetLists()).Returns(
                new List<ProfilerList>()
                {
                    new ProfilerList("1", "a", new List<string>() { "one", "two" }),
                    new ProfilerList("2", "b", new List<string>() { "three", "four" }),
                    new ProfilerList("3", "c", new List<string>() { "five", "six" })
                }
            );

            var tokenizer = new PunctuationTokenizer();

            var sut = new ProfileGenerator(mockListReader.Object, tokenizer, tokenizer);
            var result = sut.Profile(string.Join(' ', words));

            foreach (var word in words)
            {
                Assert.Contains(word, result.ParagraphHtml);
            }
        }

        [Fact]
        public void CountWords()
        {
            var words = new string[] { "one", "two!", "three,", "four?", "five:", "%six" };

            var mockListReader = new Mock<IProfilerListReader>();
            mockListReader.Setup(lr => lr.GetLists()).Returns(
                new List<ProfilerList>()
                {
                    new ProfilerList("1", "a", new List<string>() { "one", "two" }),
                    new ProfilerList("2", "b", new List<string>() { "three", "four" }),
                    new ProfilerList("3", "c", new List<string>() { "five", "six" })
                }
            );

            var tokenizer = new PunctuationTokenizer();

            var sut = new ProfileGenerator(mockListReader.Object, tokenizer, tokenizer);
            var result = sut.Profile(string.Join(' ', words));

            Assert.Equal(6, result.TotalWordCount);
        }

        [Fact]
        public void BuildParagraphCorrectly()
        {
            var input = "one" + System.Environment.NewLine + " two!";
            
            var mockListReader = new Mock<IProfilerListReader>();
            mockListReader.Setup(lr => lr.GetLists()).Returns(
                new List<ProfilerList>()
                {
                    new ProfilerList("1", "a", new List<string>() { "one", "two" })
                }
            );
            var tokenizer = new PunctuationTokenizer();

            var sut = new ProfileGenerator(mockListReader.Object, tokenizer, tokenizer);
            var result = sut.Profile(input);

            var expectedResult = ProfilerHtmlBuilder.BuildWordSpan("a", "one");
            expectedResult += "<br />";
            expectedResult += ProfilerHtmlBuilder.BuildWordSpan("a", "two");
            expectedResult += "! ";

            Assert.Equal(expectedResult, result.ParagraphHtml);
        }

        [Fact]
        public void BuildListTableResultCorrectly()
        {
            var input = "one! one? one:" + System.Environment.NewLine;

            var mockListReader = new Mock<IProfilerListReader>();
            mockListReader.Setup(lr => lr.GetLists()).Returns(
                new List<ProfilerList>()
                {
                    new ProfilerList("1", "a", new List<string>() { "one", "two" })
                }
            );
            var tokenizer = new PunctuationTokenizer();

            var sut = new ProfileGenerator(mockListReader.Object, tokenizer, tokenizer);
            var result = sut.Profile(input);

            var wordCount = new Dictionary<string, int>();
            wordCount["one"] = 3;
            var expectedResult = ProfilerHtmlBuilder.GetColumnRows(wordCount, "a");

            Assert.Equal(expectedResult.Single().RowHtml, result.TableResult["1"].Rows.Single().RowHtml);
            Assert.Equal("100%", result.TableResult["1"].Percentage);
        }


        [Fact]
        public void BuildOffListTableResultCorrectly()
        {
            var input = "one! one?" + System.Environment.NewLine + "foo foo!";

            var mockListReader = new Mock<IProfilerListReader>();
            mockListReader.Setup(lr => lr.GetLists()).Returns(
                new List<ProfilerList>()
                {
                    new ProfilerList("1", "a", new List<string>() { "one", "two" })
                }
            );
            var tokenizer = new PunctuationTokenizer();

            var sut = new ProfileGenerator(mockListReader.Object, tokenizer, tokenizer);
            var result = sut.Profile(input);

            var wordCount = new Dictionary<string, int>();
            wordCount["foo"] = 2;
            var expectedResult = ProfilerHtmlBuilder.GetColumnRows(wordCount, "profilerOffList");

            Assert.Equal(expectedResult.Single().RowHtml, result.TableResult["Off List"].Rows.Single().RowHtml);
            Assert.Equal("50%", result.TableResult["Off List"].Percentage);
        }

    }
}
