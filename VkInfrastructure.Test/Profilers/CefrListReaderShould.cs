using System.Linq;
using VkInfrastructure.Profilers;
using Xunit;

namespace VkInfrastructure.Test.Profilers
{
    public class CefrListReaderShould
    {
        [Fact]
        public void ReadAllLists()
        {
            var sut = new CefrListReader();
            var result = sut.GetLists();

            Assert.NotNull(result.SingleOrDefault(l => l.Name == "A1"));
            Assert.NotNull(result.SingleOrDefault(l => l.Name == "B1"));
            Assert.NotNull(result.SingleOrDefault(l => l.Name == "C1"));
            Assert.NotNull(result.SingleOrDefault(l => l.Name == "A2"));
            Assert.NotNull(result.SingleOrDefault(l => l.Name == "B2"));
            Assert.NotNull(result.SingleOrDefault(l => l.Name == "C2"));

        }
    }
}
