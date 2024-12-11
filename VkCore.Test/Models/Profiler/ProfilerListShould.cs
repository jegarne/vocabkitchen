using System;
using System.Collections.Generic;
using System.Text;
using VkCore.Models.Profiler;
using Xunit;

namespace VkCore.Test.Models.Profiler
{
    public class ProfilerListShould
    {
        [Fact]
        public void ReturnTrueWhenAWordIsAdded()
        {
            var wordList = new List<string>() { "one", "two", "three" };

            var sut = new ProfilerList("", "", wordList);

            Assert.True(sut.AddWord("one"));            
        }

        [Fact]
        public void ReturnFalseWhenAWordIsNotFound()
        {
            var wordList = new List<string>() { "one", "two", "three" };

            var sut = new ProfilerList("", "", wordList);

            Assert.False(sut.AddWord("false"));
        }

        [Fact]
        public void AddAWordToTheResultsDictionary()
        {
            var wordList = new List<string>() { "one", "two", "three" };

            var sut = new ProfilerList("", "", wordList);
            sut.AddWord("one");

            Assert.True(sut.ResultsDictionary.ContainsKey("one"));
            Assert.Equal(1, sut.ResultsDictionary["one"]);
        }


        [Fact]
        public void IncrementsWordCountInTheResultsDictionary()
        {
            var wordList = new List<string>() { "one", "two", "three" };

            var sut = new ProfilerList("", "", wordList);
            sut.AddWord("one");
            sut.AddWord("one");
            sut.AddWord("one");

            Assert.Equal(3, sut.ResultsDictionary["one"]);
        }
    }
}
