using System.Threading;
using System.Threading.Tasks;
using VkCore.Requests.Profiler;
using VkInfrastructure.RequestHandlers.Profiler;
using Xunit;

namespace VkInfrastructure.Test.Requests.Profiler
{
    public class ProfilerRequestHandlerShould
    {
        readonly CancellationToken _token;

        public ProfilerRequestHandlerShould()
        {
            _token = new CancellationToken();
        }

        [Fact]
        public async Task ProfileCefrRequests()
        {
            var request = new ProfilerRequest()
            {
                ProfilerType = "cefr",
                InputText = "about accident abroad absorb acknowledge backward"
            };

            var sut = new ProfilerRequestHandler();
            var result = await sut.Handle(request, _token);

            Assert.Contains("profilerA1Word", result.ParagraphHtml);
            Assert.Contains("profilerA2Word", result.ParagraphHtml);
            Assert.Contains("profilerB1Word", result.ParagraphHtml);
            Assert.Contains("profilerB2Word", result.ParagraphHtml);
            Assert.Contains("profilerC1Word", result.ParagraphHtml);
            Assert.Contains("profilerC2Word", result.ParagraphHtml);
        }

        [Fact]
        public async Task ProfileAwlRequests()
        {
            var request = new ProfilerRequest()
            {
                ProfilerType = "awl",
                InputText = "investor"
            };

            var sut = new ProfilerRequestHandler();
            var result = await sut.Handle(request, _token);

            Assert.Contains("profilerAwlWord", result.ParagraphHtml);

        }

        [Fact]
        public async Task ReturnEmptyResult()
        {
            var request = new ProfilerRequest()
            {
                ProfilerType = "foo",
                InputText = "some test text"
            };

            var sut = new ProfilerRequestHandler();
            var result = await sut.Handle(request, _token);

            Assert.True(string.IsNullOrEmpty(result.ParagraphHtml));
        }
    }
}
