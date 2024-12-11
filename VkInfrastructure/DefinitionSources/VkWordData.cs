using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VkCore.Builders;
using VkCore.Dtos;
using VkCore.Interfaces;
using VkInfrastructure.Data;

namespace VkInfrastructure.DefinitionSources
{
    public class VkWordData : IDefinitionSource
    {
        private readonly VkDbContext _ctx;
        private readonly string _requestingUserId;

        public VkWordData(VkDbContext ctx, string requestingUserId)
        {
            _ctx = ctx;
            _requestingUserId = requestingUserId;
        }

        public async Task<IEnumerable<DefinitionDto>> GetEntries(string word)
        {
            var dtos = new List<DefinitionDto>();
            var wordEntry = await _ctx.Words.FirstOrDefaultAsync(w => w.Word == word);
            if (wordEntry == null) return dtos;

            var annotations = _ctx.Annotations.Where(x => x.WordEntryId == wordEntry.Id)
                .Include(y => y.AnnotationContexts);

           foreach (var a in await annotations.ToListAsync())
           {
               dtos.Add(DefinitionDtoBuilder.FromAnnotation(a, _requestingUserId));
           }

           return dtos;
        }
    }
}
