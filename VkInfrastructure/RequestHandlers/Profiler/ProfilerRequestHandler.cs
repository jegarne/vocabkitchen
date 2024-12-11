using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VkCore.Models.Profiler;
using VkCore.Requests.Profiler;
using VkInfrastructure.Profilers;

namespace VkInfrastructure.RequestHandlers.Profiler
{
    public class ProfilerRequestHandler : IRequestHandler<ProfilerRequest, ProfilerResult>
    {

        public Task<ProfilerResult> Handle(ProfilerRequest request, CancellationToken cancellationToken)
        {
            switch (request.ProfilerType)
            {
                case "cefr":
                    return Task.FromResult(new CefrProfiler().Profile(request.InputText));
                case "awl":
                    return Task.FromResult(new AwlProfiler().Profile(request.InputText));
                case "nawl":
                    return Task.FromResult(new NawlProfiler().Profile(request.InputText));
                default:
                    return Task.FromResult(new ProfilerResult());
            }
        }
    }
}
