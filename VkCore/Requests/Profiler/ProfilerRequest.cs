using MediatR;
using VkCore.Models.Profiler;

namespace VkCore.Requests.Profiler
{
    public class ProfilerRequest : IRequest<ProfilerResult>
    {
        public string ProfilerType { get; set; }
        public string InputText { get; set; }
    }
}
