using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VkCore.Dtos;
using VkCore.Interfaces;
using VkCore.Requests.Word;
using VkCore.SharedKernel;
using VkInfrastructure.Data;
using VkInfrastructure.DefinitionSources;
using VkInfrastructure.Extensions;

namespace VkInfrastructure.RequestHandlers.Word
{
    class GetDefinitionsRequestHandler : IRequestHandler<GetDefinitionsRequest, DtoResult<IEnumerable<DefinitionDto>>>
    {
        private readonly VkDbContext _context;
        private readonly DtoResult<IEnumerable<DefinitionDto>> _response;

        public GetDefinitionsRequestHandler(VkDbContext context)
        {
            _context = context;
            _response = new DtoResult<IEnumerable<DefinitionDto>>();
        }

        public async Task<DtoResult<IEnumerable<DefinitionDto>>> Handle(GetDefinitionsRequest request, CancellationToken cancellationToken)
        {
            var preResult = new List<DefinitionDto>();

            var sources = new List<IDefinitionSource>()
            {
                new VkWordData(_context, request.RequestingUserId)
                , new OwlBotApi()
                , new WordnikApi()
            };
            
            foreach(var source in sources)
            {
                preResult.AddRange(await source.GetEntries(request.Word));
            }
           
            _response.SetValue(preResult.DistinctBy(a => a.Value));

            return _response;
        }
    }
}
