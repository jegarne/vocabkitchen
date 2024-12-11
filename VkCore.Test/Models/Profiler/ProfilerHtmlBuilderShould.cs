using System;
using System.Collections.Generic;
using VkCore.Models.Profiler;
using Xunit;

namespace VkCore.Test.Models.Profiler
{
    public class ProfilerHtmlBuilderShould
    {
        [Fact]
        public void GetColumnHtml()
        {
            var resultsDictionary = new Dictionary<string, int>();
            resultsDictionary.Add("foo", 1);
            resultsDictionary.Add("bar", 2);

            var result = ProfilerHtmlBuilder.GetColumnRows(resultsDictionary, "foo-style");

            Assert.Contains("foo", result.Find(r => r.Occurrences == 1).RowHtml);
            Assert.Contains("bar", result.Find(r => r.Occurrences == 2).RowHtml);
        }

        [Fact]
        public void BuildWordSpan()
        {

            var result = ProfilerHtmlBuilder.BuildWordSpan("foo-style", "foo");

            Assert.Contains("<span class='foo-style'>foo</span>", result);
        }

        [Fact]
        public void GetPercentage()
        {
            var resultsDictionary = new Dictionary<string, int>();
            resultsDictionary.Add("foo", 1);
            resultsDictionary.Add("bar", 2);

            var result = ProfilerHtmlBuilder.GetPercentage(resultsDictionary, 10);

            Assert.Equal("30%", result);
        }

        [Fact]
        public void RoundCorrectly()
        {
            var result = ProfilerHtmlBuilder.RoundPercentage(5, 76);

            Assert.Equal("7%", result);
        }
    }
}
