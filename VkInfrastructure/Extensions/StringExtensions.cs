using System;
using System.Text.RegularExpressions;

namespace VkInfrastructure.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Get the first several words.
        /// </summary>
        public static string FirstWords(this string input, int numberWords)
        {
            try
            {
                // Number of words we still want to display.
                int words = numberWords;
                // Loop through entire summary.
                for (int i = 0; i < input.Length; i++)
                {
                    // Increment words on a space.
                    if (input[i] == ' ')
                    {
                        words--;
                    }
                    // If we have no more words to display, return the substring.
                    if (words == 0)
                    {
                        return input.Substring(0, i);
                    }
                }
            }
            catch (Exception)
            {
                // Log the error.
            }
            return string.Empty;
        }

        public static string RemoveXml(this string input)
        {
            if (string.IsNullOrEmpty(input)) return input;

            return Regex.Replace(input, "<[^>]+>", string.Empty);
        }
    }
}