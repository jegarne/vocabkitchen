using VkCore.Extensions;
using Xunit;

namespace VkCore.Test.Extensions
{
    public class StringExtensionsShould
    {
        [Theory]
        [InlineData(0, "cars")]
        [InlineData(1, "car")]
        [InlineData(15, "cars")]
        public void PluralizeCorrectly(int count, string expected)
        {
            var sut = "car";
            var result = sut.Pluralize(count, "cars");
            Assert.Equal(expected, result);
        }


        [Theory]
        [InlineData("cars!", "cars")]
        [InlineData("cars cars", "cars cars")]
        [InlineData("@#cars,^ 8", "cars 8")]
        [InlineData("car's", "car's")]
        public void RemoveNonAlphaNumeric(string input, string expected)
        {
            var result = input.RemoveNonAlphaNumeric();
            Assert.Equal(expected, result);
        }
    }
}
