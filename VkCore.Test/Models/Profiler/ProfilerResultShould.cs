using System;
using System.Collections.Generic;
using System.Text;
using VkCore.Models.Profiler;
using Xunit;

namespace VkCore.Test.Models.Profiler
{
    public class ProfilerResultShould
    {
        private ProfilerResult _sut = new ProfilerResult();

        public ProfilerResultShould()
        {
            _sut.TableResult = new Dictionary<string, ProfilerTableResult>();

            // 4 rows
            _sut.TableResult["A1"] = new ProfilerTableResult()
            {
                Percentage = "1",
                Rows = new List<ProfilerTableRow> {
                    new ProfilerTableRow() { Occurrences = 2, RowHtml = "two" },
                    new ProfilerTableRow() { Occurrences = 3, RowHtml = "three" },
                    new ProfilerTableRow() { Occurrences = 5, RowHtml = "five" },
                    new ProfilerTableRow() { Occurrences = 4, RowHtml = "four" }
                }
            };

            // 5 rows
            _sut.TableResult["A2"] = new ProfilerTableResult()
            {
                Percentage = "1",
                Rows = new List<ProfilerTableRow> {
                    new ProfilerTableRow() { Occurrences = 5, RowHtml = "foo" },
                    new ProfilerTableRow() { Occurrences = 5, RowHtml = "foo" },
                    new ProfilerTableRow() { Occurrences = 5, RowHtml = "foo" },
                    new ProfilerTableRow() { Occurrences = 5, RowHtml = "foo" },
                    new ProfilerTableRow() { Occurrences = 5, RowHtml = "foo" }
                }
            };
        }

        [Fact]
        public void GetTheHighestRowCount()
        {
            Assert.Equal(5, _sut.GetCefrRowCount());
        }

        [Fact]
        public void BuildCefrWordDocContent()
        {
            var result = _sut.GetCefrDoc();
            Assert.NotNull(result);
        }
    }
}
