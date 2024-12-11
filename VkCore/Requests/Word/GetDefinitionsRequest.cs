using System.Collections.Generic;
using MediatR;
using VkCore.Dtos;
using VkCore.SharedKernel;

namespace VkCore.Requests.Word
{
    public class GetDefinitionsRequest : IRequest<DtoResult<IEnumerable<DefinitionDto>>>
    {
        public GetDefinitionsRequest(string word, string requestingUserId)
        {
            Word = word;
            RequestingUserId = requestingUserId;
        }

        public string Word { get; set; }
        public string RequestingUserId { get; set; }
    }
}
