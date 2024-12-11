using System.Linq;
using System.Threading.Tasks;
using VkInfrastructure.DefinitionSources;
using Xunit;

namespace VkInfrastructure.Test.DictionaryApis
{
    public class WordnikApiShould
    {
        [Fact]
        public async Task MapResultsToWordEntries()
        {
            var sut = new WordnikApi(new[] { "ahd-5" });

            var result = await sut.GetEntries("test");

            Assert.Equal(13, result.Count());
        }
    }
}
