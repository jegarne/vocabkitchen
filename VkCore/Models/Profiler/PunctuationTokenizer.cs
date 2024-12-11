using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;
using VkCore.Interfaces;

namespace VkCore.Models.Profiler
{
    public static class Placeholders
    {
        public static string LineBreak => " 00linebreak00 ";
        public static string FullStop => " 00fullstop00 ";
        public static string Decimal => " 00decimal00 ";

        public static string Comma => " 00comma00 ";
        public static string Colon => " 00colon00 ";
        public static string TimeColon => " 00timecolon00 ";
        public static string QuestionMark => " 00questionmark00 ";
        public static string Exclamation => " 00exclamationpoint00 ";
        public static string Percent => " 00percentsign00 ";
        public static string Hyphen => " 00hyphen00 ";
        public static string CurlyQuote => " 00curlyquote00 ";
        public static string Emdash => " 00emdash00 ";
        public static string LeftSingleQuote => " 00leftsinglequote00 ";
        public static string RightSingleQuote => " 00rightsinglequote00 ";
        public static string Apostrophe => " 00apostrophe00 ";
        public static string RightDoubleQuote => " 00rightdoublequote00 ";
        public static string LeftDoubleQuote => " 00leftdoublequote00 ";
        public static string PossessiveCurlyQuote => " 00possessivecurlyquote00 ";
        public static string OpenQuote => " 00openquote00 ";
        public static string CloseQuote => " 00closequote00 ";
        public static string OpenParenthesis => " 00openparenthesis00 ";
        public static string CloseParenthesis => " 00closeparenthesis00 ";
        public static string SemiColon => " 00semicolon00 ";
        public static string LessThan => " 00lessthan00 ";
        public static string GreaterThan => " 00greaterthan00 ";
        public static string QuotedComma => " 00quotedcomma00 ";
        public static string QuotedQuestion => " 00quotedquestion00 ";


        public static Dictionary<string, string[]> Mappings = new Dictionary<string, string[]>()
        {
            {"\\r\\n|\\n", new [] {Placeholders.LineBreak, "<br />"}},
            {"\\. ", new [] {Placeholders.FullStop, ". "}},
            {"\\.", new [] {Placeholders.Decimal, "."}},
            {"\\, ", new [] {Placeholders.Comma, ", "}},
            {"\\,", new [] {Placeholders.QuotedComma, ","}},
            {"\\: ", new [] {Placeholders.Colon, "&#58; "}},
            {"\\:", new [] {Placeholders.TimeColon, "&#58;"}},
            {"\\? ", new [] {Placeholders.QuestionMark, "? "}},
            {"\\!", new [] {Placeholders.Exclamation, "! "}},
            {"\\%", new [] {Placeholders.Percent, "% "}},
            {"\\—", new [] {Placeholders.Emdash, " — "}},
            {"\\’ ", new [] {Placeholders.PossessiveCurlyQuote, "’ "}},
            {"\\’", new [] {Placeholders.CurlyQuote, "’"}},
            {"\\-", new [] {Placeholders.Hyphen, "-"}},
            {" \\'", new [] {Placeholders.LeftSingleQuote, " &#39;"}},
            {"\\' ", new [] {Placeholders.RightSingleQuote, "&#39; "}},
            {"\\'", new [] {Placeholders.Apostrophe, "&#39;"}},
            {"\\\" ", new [] {Placeholders.RightDoubleQuote, "&quot; "}},
            {" \\\"", new [] {Placeholders.LeftDoubleQuote, " &quot;"}},
            {" \\“", new [] {Placeholders.OpenQuote, " “"}},
            {"\\” ", new [] {Placeholders.CloseQuote, "” "}},
            {" \\(", new [] {Placeholders.OpenParenthesis, " ("}},
            {"\\) ", new [] {Placeholders.CloseParenthesis, ") "}},
            {"\\; ", new [] {Placeholders.SemiColon, "&#59; "}},
            {"\\>", new [] {Placeholders.GreaterThan, "&gt;"}},
            {"\\<", new [] {Placeholders.LessThan, "&lt;"}},
            {"\\?", new [] {Placeholders.QuotedQuestion, "?"}}

        };
    }

    public class PunctuationTokenizer : ITokenizer, IHtmlEntityConverter
    {
        public string[] Tokenize(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return new string[0];

            foreach (var mapping in Placeholders.Mappings)
            {
                input = PunctuationRegex(input, mapping.Key, mapping.Value[0]);
            }

            // tokenizes input text and removes empty strings
            string[] textWords = Regex.Split(input, "[^a-zA-Z0-9]+");
            var result2 = textWords.Where(item => !string.IsNullOrEmpty(item));

            return result2.ToArray();
        }

        private string PunctuationRegex(string input, string indentifyingString, string placeholder)
        {
            var regex = new Regex(indentifyingString);
            return regex.Replace(input, placeholder);
        }

        public string ConvertToHtmlEntity(string inputWord)
        {
            //note on html output: the inner html should not contain any spaces
            //yet there must be one space following the closing tag in order for the
            //resulting paragraph to display correctly

            foreach (var mapping in Placeholders.Mappings)
            {
                if (inputWord == mapping.Value[0].Trim())
                    return mapping.Value[1];
            }

            return "";
        }
    }
}
