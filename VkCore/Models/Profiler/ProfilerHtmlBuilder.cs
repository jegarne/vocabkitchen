using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Globalization;

namespace VkCore.Models.Profiler
{
    public static class ProfilerHtmlBuilder
    {
        public static List<ProfilerTableRow> GetColumnRows(Dictionary<string, int> resultsDictionary, string cssClass)
        {
            var result = new List<ProfilerTableRow>();
            foreach (KeyValuePair<string, int> kv in resultsDictionary.OrderBy(x => x.Key))
            {
                result.Add(new ProfilerTableRow()
                {
                    Occurrences = kv.Value,
                    RowHtml = BuildRowHtml(kv.Key, cssClass)
                });
            }

            return result;
        }

        public static string BuildWordSpan(string cssClass, string word)
        {
            return $"<span class='{cssClass}'>{word}</span>";
        }

        public static string GetPercentage(Dictionary<string, int> dict, int denominator)
        {
            int dictSum = dict.Sum(x => x.Value);
            return RoundPercentage(dictSum, denominator);
        }

        public static string RoundPercentage(double numerator, int denominator)
        {
            NumberFormatInfo percentageFormat = new NumberFormatInfo
            {
                PercentPositivePattern = 1,
                PercentNegativePattern = 1
            };
            if (denominator == 0) return   0m.ToString("P0", percentageFormat);

            var average = numerator / denominator;
            decimal dec = (decimal)average;
            decimal rounded = decimal.Round(dec, 2);
            return rounded.ToString("P0", percentageFormat); // "-12.30%" (in en-us)
        }

        private static string BuildRowHtml(string word, string cssClass)
        {
            return $"<span class='word {cssClass}'>{word}</span>";
        }

    }
}
