using System.Linq;
using VkInfrastructure.Profilers;
using Xunit;

namespace VkInfrastructure.Test.Profilers
{
    public class AwlListReaderShould
    {
        [Fact]
        public void ReadAllLists()
        {
            var sut = new AwlListReader();
            var result = sut.GetLists();

            Assert.NotNull(result.SingleOrDefault(l => l.Name == "Awl"));

        }
    }
}
