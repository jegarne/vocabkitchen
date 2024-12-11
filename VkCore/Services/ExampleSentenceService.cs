using PragmaticSegmenterNet;
using System;
using System.Collections.Generic;
using System.Linq;

namespace VkCore.Services
{
    public class ExampleSentenceService
    {
        private Dictionary<int, int> _sentenceIndexes;
        private IReadOnlyList<string> _sentences;

        public ExampleSentenceService(string text)
        {
            _sentences = SplitSentences(text);
            _sentenceIndexes = IndexSentences(_sentences, text);
        }

        public string GetSentenceAtIndex(int startIndex)
        {
            if (_sentenceIndexes.Max(x => x.Value) < startIndex) return null;

            var targetIndex = _sentenceIndexes.First(x => x.Value >= startIndex).Key;
            return _sentences[targetIndex].Trim();
        }

        private IReadOnlyList<string> SplitSentences(string text)
        {
            return Segmenter.Segment(text, Language.English, false);
        }

        private Dictionary<int, int> IndexSentences(IReadOnlyList<string> sentences, string text)
        {
            var sentenceIndexes = new Dictionary<int, int>();

            // we only have one sentence
            // was seeing weird behavior if text doesn't have a period
            if(sentences.Count() == 1)
            {
                sentenceIndexes[0] = text.Length;
                return sentenceIndexes;
            }

            var whiteSpaceLength = 0;
            var sentenceIndex = 0;
            for (int i = 0; i < text.Length;)
            {
                var nextChar = text[i];
                if (Char.IsWhiteSpace(nextChar))
                {
                    whiteSpaceLength++;

                    if (i == text.Length - 1)
                    {
                        sentenceIndexes[sentenceIndex - 1] = sentenceIndexes[sentenceIndex - 1] + whiteSpaceLength;
                        break;
                    }

                    i++;
                    continue;
                }
                               
                // not whitespace so add sentence length
                var sentenceLength = sentences[sentenceIndex].Length;
                i += sentenceLength;

                if (sentenceIndex == 0)
                {
                    sentenceIndexes[sentenceIndex] = whiteSpaceLength + sentences[sentenceIndex].Length;
                }
                else
                {
                    sentenceIndexes[sentenceIndex] = sentenceIndexes[sentenceIndex - 1] + whiteSpaceLength + sentences[sentenceIndex].Length;
                }

                sentenceIndex++;
                whiteSpaceLength = 0;
            }

            return sentenceIndexes;
        }

        public Dictionary<int, int> SentenceIndexes => _sentenceIndexes;
        public IReadOnlyList<string> Sentences => _sentences;

    }
}
