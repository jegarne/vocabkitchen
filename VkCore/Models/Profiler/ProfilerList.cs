using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

namespace VkCore.Models.Profiler
{
    public class ProfilerList
    {
        private List<string> _wordList;

        public ProfilerList(string name, string cssClass, List<string> wordList)
        {
            Name = name;
            CssClass = cssClass;
            _wordList = wordList;
        }

        public string Name { get; }
        public string CssClass { get; }
        public IEnumerable<string> WordList => _wordList;

        public Dictionary<string, int> ResultsDictionary { get; set; } = new Dictionary<string, int>();

        public bool AddWord(string inputWord)
        {
            if (_wordList.Contains(inputWord, StringComparer.OrdinalIgnoreCase))
            {
                string lowerInputWord = inputWord.ToLower();
                if (ResultsDictionary.ContainsKey(lowerInputWord))
                {
                    // If the word is in the dictionary, increment value
                    ResultsDictionary[lowerInputWord]++;
                }
                else
                {  // If the word is not in the dictionary, add it and set is value to 1
                    ResultsDictionary.Add(lowerInputWord, 1);
                }

                return true;
            }

            return false;
        }
    }
}
