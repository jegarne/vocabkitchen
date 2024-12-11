using System;
using System.IO;
using VkInfrastructure.Profilers;
using Xunit;

namespace VkInfrastructure.Test.Profilers
{
    public class ProfilerListReaderShould
    {
        [Fact]
        public void ShouldReadWordList()
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "TestWordList.txt");
            File.WriteAllLines(path, new string[] { "one", "two", "three" });

            var sut = new ProfilerListReader(Directory.GetCurrentDirectory());
            var result = sut.GetWordList("TestWordList.txt");

            Assert.Contains("one", result);
            Assert.Contains("two", result);
            Assert.Contains("three", result);

            File.Delete(path);
        }


        [Fact]
        public void ThrowExceptionWhenFileIsNotFound()
        {
            Action a = () => new ProfilerListReader(Directory.GetCurrentDirectory()).GetWordList("MissingList.txt");

            Assert.Throws<FileNotFoundException>(a);
        }
    }
}
