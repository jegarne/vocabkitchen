using System.Linq;
using System.Threading.Tasks;
using VkInfrastructure.DefinitionSources;
using Xunit;

namespace VkInfrastructure.Test.DictionaryApis
{
    public class OwlBotApiShould
    {
        [Fact]
        public async Task MapResultsToWordEntries()
        {
            var sut = new OwlBotApi();

            var result = await sut.GetEntries("test");

            Assert.Equal(3, result.Count());
        }
    }
}
