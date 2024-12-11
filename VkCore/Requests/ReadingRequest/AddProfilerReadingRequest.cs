using MediatR;
using VkCore.Dtos;
using VkCore.SharedKernel;

namespace VkCore.Requests.ReadingRequest
{
    public class AddProfilerReadingRequest : IRequest<DtoResult<ReadingDto>>
    {
        public AddProfilerReadingRequest()
        { }

        public AddProfilerReadingRequest(string userId, string title, string text)
        {
            UserId = userId;
            Text = text;
        }

        public string UserId { get; set; }
        public string Text { get; set; }
    }
}
