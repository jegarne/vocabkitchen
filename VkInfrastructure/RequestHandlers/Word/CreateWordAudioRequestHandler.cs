using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Amazon;
using Amazon.Lambda;
using Amazon.Lambda.Model;
using Newtonsoft.Json;
using VkCore.Requests.Word;
using VkCore.SharedKernel;

namespace VkInfrastructure.RequestHandlers.Word
{
    public class CreateWordAudioRequestHandler : IRequestHandler<CreateWordAudioRequest, DtoResult<string>>
    {
        private readonly DtoResult<string> _response;

        public CreateWordAudioRequestHandler()
        {
            _response = new DtoResult<string>();
        }

        public async Task<DtoResult<string>> Handle(CreateWordAudioRequest request, CancellationToken cancellationToken)
        {
            var lambdaClient = new AmazonLambdaClient(RegionEndpoint.USEast1);

            var invokeRequest = new InvokeRequest
            {
                FunctionName = "VkLambdas-CreateWordAudio-1",
                InvocationType = InvocationType.RequestResponse,
                Payload = $"\"{request.Word}\""
            };

            var invokeResponse = await lambdaClient.InvokeAsync(invokeRequest, cancellationToken);

            if (invokeResponse.StatusCode == 200)
            {
                return _response;
            }

            _response.AddError($"Unable to create audio for {request.Word}");
            return _response;
        }
    }
}
