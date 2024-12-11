using System;
using System.Collections.Generic;
using System.Text;
using VkCore.Models.Profiler;

namespace VkCore.Interfaces
{
    public interface IProfiler
    {
        ProfilerResult Profile(string input);
    }
}
