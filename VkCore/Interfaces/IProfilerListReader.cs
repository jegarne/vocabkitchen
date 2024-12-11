using System.Collections.Generic;
using VkCore.Models.Profiler;

namespace VkCore.Interfaces
{
    public interface IProfilerListReader
    {
        List<ProfilerList> GetLists();
    }
}
