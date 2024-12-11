using System;
using System.Collections.Generic;

namespace VkCore.Interfaces
{
    public interface IResponseErrorLogger
    {
        IEnumerable<string> Errors { get; }
    }
}
