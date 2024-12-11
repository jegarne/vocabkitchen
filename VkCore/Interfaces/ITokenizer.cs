using System;
using System.Collections.Generic;
using System.Text;

namespace VkCore.Interfaces
{
    public interface ITokenizer
    {
        string[] Tokenize(string input);
    }
}
