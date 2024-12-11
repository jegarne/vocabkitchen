using System.Collections.Generic;
using System.Threading.Tasks;
using VkCore.Dtos;
using VkCore.Models.Word;

namespace VkCore.Interfaces
{
    public interface IDefinitionSource
    {
        Task<IEnumerable<DefinitionDto>> GetEntries(string word);
    }
}
