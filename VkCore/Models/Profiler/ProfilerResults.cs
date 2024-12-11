using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VkCore.Models.Profiler
{
    public class ProfilerResult
    {
        public int TotalWordCount { get; set; }
        public string ParagraphHtml { get; set; }
        public Dictionary<string, ProfilerTableResult> TableResult { get; set; } = new Dictionary<string, ProfilerTableResult>();

        public int GetCefrRowCount()
        {
            var rowCount = this.TableResult["A1"].Rows.Count();
            var levels = new List<string>() { "A2", "B1", "B2", "C1", "C2" };

            foreach (var level in levels)
            {
                if (this.TableResult.ContainsKey(level))
                {
                    if (this.TableResult[level]?.Rows?.Count() > rowCount)
                    {
                        rowCount = this.TableResult[level].Rows.Count();
                    }
                }
            }

            return rowCount;
        }

        public string GetCefrDoc()
        {
            StringBuilder htmlResult = new StringBuilder();
            htmlResult.Append("<html><head><style type='text/css'>");
            // get css styling and add to document
            var cssStyles = new Dictionary<string, string>();
            cssStyles.Add("profilerA1Word", "#0099CC");
            cssStyles.Add("profilerA2Word", "#00bb00");
            cssStyles.Add("profilerB1Word", "#ff9900");
            cssStyles.Add("profilerB2Word", "#B30000");
            cssStyles.Add("profilerC1Word", "#D733FF");
            cssStyles.Add("profilerC2Word", "#DB7093");
            var levels = new List<string>() { "A1", "A2", "B1", "B2", "C1", "C2" };

            foreach (var style in cssStyles)
            {
                htmlResult.Append($".{style.Key} {{ color: {style.Value}; font - weight: bold; }}");
            }

            htmlResult.Append("</style><head>");
            htmlResult.Append("<table>");
            htmlResult.Append("<tr>");
            foreach (var level in levels)
            {
                if (this.TableResult.ContainsKey(level))
                {
                    htmlResult.Append($@"<th class='profiler{level}Word' style='text-align:left;'>{level}: {this.TableResult[level].Percentage}</th>");
                    this.TableResult[level].Rows = TableResult[level].Rows.OrderByDescending(x => x.Occurrences).ToList();
                }
            }
            htmlResult.Append(@"<tr>");

            var rowCount = this.GetCefrRowCount();
            for (int i = 0; i < rowCount; i++)
            {
                htmlResult.Append("<tr>");
                foreach (var level in levels)
                {
                    if (this.TableResult.ContainsKey(level))
                    {
                        if (this.TableResult[level].Rows.Count() > i)
                        {
                            htmlResult.Append($@"<td class='profiler{level}Word'>{this.TableResult[level].Rows[i].Occurrences}: {this.TableResult[level].Rows[i].RowHtml}</td>");
                        }
                        else
                        {
                            htmlResult.Append("<td></td>");
                        }
                    }
                }
                htmlResult.Append(@"<tr>");
            }

            htmlResult.Append(@"</table>");
            htmlResult.Append("<br />");
            htmlResult.Append(this.ParagraphHtml);
            htmlResult.Append("</html>");
            return htmlResult.ToString();
        }
    }

    public class ProfilerTableResult
    {
        public string Percentage { get; set; }
        public List<ProfilerTableRow> Rows { get; set; }
    }

    public class ProfilerTableRow
    {
        public int Occurrences { get; set; }
        public string RowHtml { get; set; }
    }
}
