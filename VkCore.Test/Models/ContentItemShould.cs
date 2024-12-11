using System;
using VkCore.Models.ReadingModel;
using Xunit;

namespace VkCore.Test.Models
{
    public class ContentItemShould
    {
        [Theory]
        [InlineData(0, "0", 0)]
        [InlineData(0, "0123", 3)]
        [InlineData(4, "4567", 7)]
        [InlineData(3, "\r\n\r\n", 4)]
        public void SetIndexesCorrectly(int start, string value, int expectedEnd)
        {
            var sut = new ContentItem(0, value);
            sut.SetFirstIndex(start);
            Assert.Equal(expectedEnd, sut.LastIndex);
        }

        [Theory]
        [InlineData(0, "012", 0, 0, "12")]
        [InlineData(0, "012", 1, 1, "02")]
        [InlineData(0, "012", 2, 2, "01")]
        [InlineData(3, "012", 3, 3, "12")]
        [InlineData(3, "012", 4, 4, "02")]
        [InlineData(3, "012", 5, 5, "01")]
        public void BackspaceContent(int firstIndex, string initialVal, int start, int end, string result)
        {
            var sut = new ContentItem(0, initialVal, "foo");
            sut.SetFirstIndex(firstIndex);
            sut.DeleteFromValue(start, end);
            Assert.Equal(result, sut.Value);
        }

        [Theory]
        [InlineData(0, 1, 1)]
        [InlineData(5, 0, 5)]
        [InlineData(100, 1, 101)]
        public void ReturnGlobalIndex(int firstIndex, int localIndex, int expected)
        {
            var sut = new ContentItem(0, "");
            sut.SetFirstIndex(firstIndex);

            var result = sut.GetGlobalIndex(localIndex);

            Assert.Equal(expected, result);
        }
    }
}
