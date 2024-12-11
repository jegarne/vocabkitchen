using AutoFixture;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading;
using VkCore.Requests.User;
using VkCore.SharedKernel;
using VkWeb.Controllers;
using Xunit;

namespace VkWeb.Test.Controllers
{
    public class AuthControllerShould
    {
        readonly CancellationToken _token;

        public AuthControllerShould()
        {
            _token = new CancellationToken();
        }

        [Fact]
        public void PassDtoToService()
        {
            var fixture = new Fixture();
            var dto = fixture.Create<UserLoginRequest>();
            var mockService = new Mock<IMediator>();

            var sut = new AuthController(mockService.Object);
            var result = sut.Login(dto, _token);

            mockService.Verify(s => s.Send(dto, _token), Times.Once);
        }

        [Fact]
        public async void ReturnOkWhenNoErrors()
        {
            var fixture = new Fixture();
            var dto = fixture.Create<UserLoginRequest>();
            var dtoResult = fixture.Create<DtoResult<string>>();
            var mockService = new Mock<IMediator>();

            mockService.Setup(s => s.Send(dto, _token)).ReturnsAsync(dtoResult);

            var sut = new AuthController(mockService.Object);
            var result = await sut.Login(dto, _token);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void ReturnBadRequestWhenErrors()
        {
            var fixture = new Fixture();
            var dto = fixture.Create<UserLoginRequest>();
            var dtoResult = fixture.Create<DtoResult<string>>();
            var mockService = new Mock<IMediator>();
            dtoResult.AddError("test", "error message");

            mockService.Setup(s => s.Send(dto, _token)).ReturnsAsync(dtoResult);

            var sut = new AuthController(mockService.Object);
            var result = await sut.Login(dto, _token);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async void ReturnErrorsInBadRequestObject()
        {
            var fixture = new Fixture();
            var dto = fixture.Create<UserLoginRequest>();
            var dtoResult = fixture.Create<DtoResult<string>>();
            var mockService = new Mock<IMediator>();
            dtoResult.AddError("test", "error message");

            mockService.Setup(s => s.Send(dto, _token)).ReturnsAsync(dtoResult);

            var sut = new AuthController(mockService.Object);
            var response = await sut.Login(dto, _token);
            var result = response as BadRequestObjectResult;

            var errorString = JsonConvert.SerializeObject(result.Value);
            var errors = JObject.Parse(errorString);
            Assert.Equal("error message", errors["test"][0]);
        }

    }
}
