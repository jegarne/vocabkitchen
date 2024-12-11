using System.Collections.Generic;
using System.IO;
using VkCore.Interfaces;
using VkCore.Models.Profiler;

namespace VkInfrastructure.Profilers
{
    public class CefrListReader : IProfilerListReader
    {
        // TODO: pull all of this from DB
        // instead of text files
        public List<ProfilerList> GetLists()
        {
            var lists = new List<ProfilerList>();

            lists.Add(new ProfilerList("A1", "profilerA1Word", new ProfilerListReader().GetWordList(GetPath("A1"))));
            lists.Add(new ProfilerList("A2", "profilerA2Word", new ProfilerListReader().GetWordList(GetPath("A2"))));

            lists.Add(new ProfilerList("B1", "profilerB1Word", new ProfilerListReader().GetWordList(GetPath("B1"))));
            lists.Add(new ProfilerList("B2", "profilerB2Word", new ProfilerListReader().GetWordList(GetPath("B2"))));

            lists.Add(new ProfilerList("C1", "profilerC1Word", new ProfilerListReader().GetWordList(GetPath("C1"))));
            lists.Add(new ProfilerList("C2", "profilerC2Word", new ProfilerListReader().GetWordList(GetPath("C2"))));

            return lists;
        }

        private string GetPath(string level)
        {
            return Path.Combine("Profilers", "WordLists", "CEFR", level + ".txt");
        }
    }

    public class CefrProfiler : IProfiler
    {
        public ProfilerResult Profile(string input)
        {
            var tokenizer = new PunctuationTokenizer();
            var pg = new ProfileGenerator(new CefrListReader(), tokenizer, tokenizer);

            return pg.Profile(input);
        }
    }
}
