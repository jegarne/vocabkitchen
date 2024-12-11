using System.Collections.Generic;
using System.Linq;
using VkCore.Models.TagModel;
using Xunit;

namespace VkCore.Test.Models
{
    public class TagShould
    {
        [Fact]
        public void MergeReadings()
        {
            var sut = new Tag("1", "tag");
            sut.AddReading("1");
            sut.AddReading("2");
            sut.AddReading("3");

            sut.MergeReadings(new List<string>() {"1", "4"});
            Assert.Equal(2, sut.Readings.Count());
            Assert.NotNull(sut.Readings.FirstOrDefault(r => r.ReadingId == "1"));
            Assert.NotNull(sut.Readings.FirstOrDefault(r => r.ReadingId == "4"));
        }

        [Fact]
        public void MergeUsers()
        {
            var sut = new Tag("1", "tag");
            sut.AddUser("1");
            sut.AddUser("2");
            sut.AddUser("3");

            sut.MergeUsers(new List<string>() { "1", "4" });
            Assert.Equal(2, sut.Users.Count());
            Assert.NotNull(sut.Users.FirstOrDefault(r => r.UserId == "1"));
            Assert.NotNull(sut.Users.FirstOrDefault(r => r.UserId == "4"));
        }
    }
}
