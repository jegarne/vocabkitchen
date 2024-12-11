using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VkCore.Models.ReadingModel;
using Xunit;

namespace VkCore.Test.Models
{
    public class DefinedContentItemFactoryShould
    {

        [Theory]
        [InlineData(0, 2, "012", "012")]
        [InlineData(0, 9, "0123456789", "0123456789")]
        public void BuildOneContentItem(int start, int end, string baseText, string i1Text)
        {
            var sut = new DefinedContentItemFactory(0, start, end, baseText, "wordId");

            var result = sut.BuildContentItems();

            Assert.Single(result);
            Assert.Equal(i1Text, result.First(x => x.BodyIndex == 0).Value);
            Assert.Equal("wordId", result.First(x => x.BodyIndex == 0).WordId);
        }

        [Theory]
        [InlineData(0, 0, 0, "012", "0", "12", 0, 1)]
        [InlineData(4, 0, 0, "012", "0", "12", 4, 5)]
        [InlineData(4, 0, 1, "012345", "01", "2345", 4, 5)]
        [InlineData(4, 0, 4, "0123456789", "01234", "56789", 4, 5)]
        [InlineData(4, 0, 1, "012", "01", "2", 4, 5)]
        [InlineData(4, 4, 5, "012345", "0123", "45", 4, 5)]
        [InlineData(4, 5, 9, "0123456789", "01234", "56789", 4, 5)]
        public void BuildTwoContentItems(int bodyIndexStart, int start, int end, string baseText, string i1Text, string i2Text, int i1Index, int i2Index)
        {
            var sut = new DefinedContentItemFactory(bodyIndexStart, start, end, baseText);

            var result = sut.BuildContentItems();

            Assert.Equal(i1Text, result.First(x => x.BodyIndex == i1Index).Value);
            Assert.Equal(i2Text, result.First(x => x.BodyIndex == i2Index).Value);
        }

        [Theory]
        [InlineData(0, 1, 1, "012", "0", "1", "2", 0, 1, 2)]
        [InlineData(0, 1, 3, "012345", "0", "123", "45", 0, 1, 2)]
        [InlineData(0, 1, 8, "0123456789", "0", "12345678", "9", 0, 1, 2)]
        public void BuildThreeContentItems(int bodyIndexStart, int start, int end, string baseText, string i1Text, string i2Text, string i3Text, int i1Index, int i2Index, int i3Index)
        {
            var sut = new DefinedContentItemFactory(bodyIndexStart, start, end, baseText);

            var result = sut.BuildContentItems();

            Assert.Equal(i1Text, result.First(x => x.BodyIndex == i1Index).Value);
            Assert.Equal(i2Text, result.First(x => x.BodyIndex == i2Index).Value);
            Assert.Equal(i3Text, result.First(x => x.BodyIndex == i3Index).Value);
        }


        [Fact]
        public void SetWordIdtoFirstContentItem()
        {
            var sut = new DefinedContentItemFactory(0, 0, 0, "012", "wordId");

            var result = sut.BuildContentItems();

            Assert.Equal("0", result.First(x => x.BodyIndex == 0).Value);
            Assert.Equal("wordId", result.First(x => x.BodyIndex == 0).WordId);
            Assert.Equal("12", result.First(x => x.BodyIndex == 1).Value);
        }

        [Fact]
        public void SetWordIdtoSecondContentItem()
        {
            var sut = new DefinedContentItemFactory(0, 2, 2, "012", "wordId");

            var result = sut.BuildContentItems();

            Assert.Equal("01", result.First(x => x.BodyIndex == 0).Value);
            Assert.Equal("wordId", result.First(x => x.BodyIndex == 1).WordId);
            Assert.Equal("2", result.First(x => x.BodyIndex == 1).Value);
        }

        [Fact]
        public void SetWordIdtoMiddleContentItem()
        {
            var sut = new DefinedContentItemFactory(0, 1, 1, "012", "wordId");

            var result = sut.BuildContentItems();

            Assert.Equal("0", result.First(x => x.BodyIndex == 0).Value);
            Assert.Equal("wordId", result.First(x => x.BodyIndex == 1).WordId);
            Assert.Equal("1", result.First(x => x.BodyIndex == 1).Value);
            Assert.Equal("2", result.First(x => x.BodyIndex == 2).Value);
        }
    }
}
