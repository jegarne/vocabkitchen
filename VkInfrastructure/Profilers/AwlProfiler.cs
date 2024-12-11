using System.Collections.Generic;
using System.IO;
using VkCore.Interfaces;
using VkCore.Models.Profiler;

namespace VkInfrastructure.Profilers
{
    public class AwlListReader : IProfilerListReader
    {
        // TODO: pull all of this from DB
        // instead of text files
        public List<ProfilerList> GetLists()
        {
            var lists = new List<ProfilerList>();
            lists.Add(new ProfilerList("Awl", "profilerAwlWord", new ProfilerListReader().GetWordList(GetPath("awl"))));

            return lists;            
        }


        private string GetPath(string level)
        {
            return Path.Combine("Profilers", "WordLists", "AWL", level + ".txt");
        }
    }

    public class AwlProfiler : IProfiler
    {
        public ProfilerResult Profile(string input)
        {
            var tokenizer = new PunctuationTokenizer();
            var pg = new ProfileGenerator(new AwlListReader(), tokenizer, tokenizer);

            return pg.Profile(input);
        }
    }
}
