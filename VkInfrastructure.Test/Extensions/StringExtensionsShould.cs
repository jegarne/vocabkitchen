using VkInfrastructure.Extensions;
using Xunit;

namespace VkInfrastructure.Test.Extensions
{
    public class StringExtensionsShould
    {
        [Theory]
        [InlineData(null, null)]
        [InlineData("", "")]
        [InlineData("test test", "test test")]
        [InlineData("the family <em>Strigidæ</em>", "the family Strigidæ")]
        [InlineData("form of <internalXref urlencoded=\"wool\">wool</internalXref>.", "form of wool.")]
        [InlineData("large moth (<spn>Erebus strix</spn>).", "large moth (Erebus strix).")]
        [InlineData("various <xref>birds of prey</xref> of the order <xref>Strigiformes</xref> that are primarily <xref>nocturnal</xref> and have", "various birds of prey of the order Strigiformes that are primarily nocturnal and have")]
        public void RemoveXml(string sut, string expected)
        {
            var result = sut.RemoveXml();

            Assert.Equal(expected, result);
        }
    }
}
