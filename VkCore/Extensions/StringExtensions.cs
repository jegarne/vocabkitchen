using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace VkCore.Extensions
{
    public static class StringExtensions
    {
        public static string Pluralize(this string singularForm, int count, string pluralForm)
        {
            return count == 1 ? singularForm : pluralForm;
        }

        public static string RemoveNonAlphaNumeric(this string input)
        {
            return Regex.Replace(input, "[^a-zA-Z0-9\\s']", String.Empty);
        }
    }
}
