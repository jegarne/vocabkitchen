using System.Collections.Generic;
using System.IO;
using VkCore.Interfaces;
using VkCore.Models.Profiler;

namespace VkInfrastructure.Profilers
{
    public class NawlListReader : IProfilerListReader
    {
        // TODO: pull all of this from DB
        // instead of text files
        public List<ProfilerList> GetLists()
        {
            var lists = new List<ProfilerList>();
            lists.Add(new ProfilerList("Nawl", "profilerAwlWord", new ProfilerListReader().GetWordList(GetPath("nawl"))));

            return lists;            
        }


        private string GetPath(string level)
        {
            return Path.Combine("Profilers", "WordLists", "NAWL", level + ".txt");
        }
    }

    public class NawlProfiler : IProfiler
    {
        public ProfilerResult Profile(string input)
        {
            var tokenizer = new PunctuationTokenizer();
            var pg = new ProfileGenerator(new NawlListReader(), tokenizer, tokenizer);

            return pg.Profile(input);
        }
    }
}
