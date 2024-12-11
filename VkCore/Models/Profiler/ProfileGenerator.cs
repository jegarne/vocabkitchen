using System.Collections.Generic;
using System.Text;
using VkCore.Interfaces;

namespace VkCore.Models.Profiler
{
    public class ProfileGenerator
    {
        private List<ProfilerList> _profilerLists = new List<ProfilerList>();
        private Dictionary<string, List<string>> _wordLists = new Dictionary<string, List<string>>();
        private StringBuilder _paragraphResult = new StringBuilder();
        private Dictionary<string, int> _offListDict = new Dictionary<string, int>();
        private string _offListClass = "profilerOffList";
        private int _totalCount = 0;
        private ITokenizer _tokenizer;
        private IHtmlEntityConverter _htmlEntityConverter;

        public ProfileGenerator(IProfilerListReader listReader, ITokenizer tokenizer, IHtmlEntityConverter entityConverter)
        {
            _profilerLists = listReader.GetLists();
            _tokenizer = tokenizer;
            _htmlEntityConverter = entityConverter;
        }

        //This returns the input text in its entirety with the frequency results
        //shown by different css classes
        public ProfilerResult Profile(string input)
        {
            // tokenize input
            string[] inputWords = _tokenizer.Tokenize(input);

            // build paragraph results output
            var justAddedPunctuation = true;
            foreach (string inputWord in inputWords)
            {
                // get html version of punctuation
                string punctuation = _htmlEntityConverter.ConvertToHtmlEntity(inputWord);

                // if it's punctuation, skip to the next word
                if (punctuation != "")
                {
                    _paragraphResult.Append(punctuation);
                    justAddedPunctuation = true;
                    continue;
                }
                // otherwise just add a space as we're between words
                if(!justAddedPunctuation)
                {
                    _paragraphResult.Append(" ");
                }

                justAddedPunctuation = false;

                // count here so we don't include punctuation
                _totalCount++;

                // check if word is in lists
                bool wasAdded = false;
                foreach (var list in _profilerLists)
                {
                    wasAdded = list.AddWord(inputWord);
                    if (wasAdded)
                    {
                        _paragraphResult.Append(ProfilerHtmlBuilder.BuildWordSpan(list.CssClass, inputWord));
                        break;
                    }
                };

                // if not add it to the off list results
                if (!wasAdded)
                {
                    _paragraphResult.Append(ProfilerHtmlBuilder.BuildWordSpan(_offListClass, inputWord));
                    string lowerInputWord = inputWord.ToLower();
                    if (_offListDict.ContainsKey(lowerInputWord))
                    {
                        _offListDict[lowerInputWord]++;
                    }
                    else
                    {
                        _offListDict.Add(lowerInputWord, 1);
                    }
                }
            }

            // prepare result
            ProfilerResult result = new ProfilerResult();
            result.TotalWordCount = _totalCount;
            result.ParagraphHtml = _paragraphResult.ToString();

            foreach (var list in _profilerLists)
            {
                var ptr = new ProfilerTableResult()
                {
                    Percentage = ProfilerHtmlBuilder.GetPercentage(list.ResultsDictionary, _totalCount),
                    Rows = ProfilerHtmlBuilder.GetColumnRows(list.ResultsDictionary, list.CssClass)
                };
                result.TableResult[list.Name] = ptr;
            }

            var offListPtr = new ProfilerTableResult()
            {
                Percentage = ProfilerHtmlBuilder.GetPercentage(_offListDict, _totalCount),
                Rows = ProfilerHtmlBuilder.GetColumnRows(_offListDict, _offListClass)
            };
            result.TableResult["Off List"] = offListPtr;

            return result;
        }
    }
}
