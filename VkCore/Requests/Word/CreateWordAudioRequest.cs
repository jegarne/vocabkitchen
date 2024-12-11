using System.Collections.Generic;
using MediatR;
using VkCore.Dtos;
using VkCore.SharedKernel;

namespace VkCore.Requests.Word
{
    public class CreateWordAudioRequest : IRequest<DtoResult<string>>
    {
        public CreateWordAudioRequest(string word)
        {
            Word = word;
        }

        public string Word { get; set; }
    }
}
