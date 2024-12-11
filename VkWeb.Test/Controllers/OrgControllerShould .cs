using AutoFixture;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading;
using VkCore.Models.Organization;
using VkCore.Requests.Organization;
using VkCore.SharedKernel;
using VkInfrastructure.Data;
using VkWeb.Controllers;
using Xunit;

namespace VkWeb.Test.Controllers
{
    public class OrgControllerShould
    {
        readonly CancellationToken _token;
        private readonly VkDbContext _context;

        public OrgControllerShould()
        {
            _token = new CancellationToken();
            _context = DbContextBuilder.GetContext();
        }

        [Fact]
        public void PassDtoToService()
        {
            var fixture = new Fixture();
            var orgDto = fixture.Create<CreateOrgRequest>();
            var mockCreateOrgService = new Mock<IMediator>();
            var mockAuthService = new Mock<IAuthorizationService>();

            var sut = new AdminController(
                mockCreateOrgService.Object, 
                _context,
                mockAuthService.Object
                );
            var result = sut.Post(orgDto, _token);

            mockCreateOrgService.Verify(s => s.Send(orgDto, _token), Times.Once);
        }

        [Fact]
        public async void ReturnOkWhenNoErrors()
        {
            var fixture = new Fixture();
            var orgDto = fixture.Create<CreateOrgRequest>();
            var dtoResult = fixture.Create<DtoResult<Org>>();
            var mockCreateOrgService = new Mock<IMediator>();
            var mockAuthService = new Mock<IAuthorizationService>();

            mockCreateOrgService.Setup(s => s.Send(orgDto, _token)).ReturnsAsync(dtoResult);

            var sut = new AdminController(
                mockCreateOrgService.Object,
                _context,
                mockAuthService.Object
                );
            var result = await sut.Post(orgDto, _token);

            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async void ReturnBadRequestWhenErrors()
        {
            var fixture = new Fixture();
            var orgDto = fixture.Create<CreateOrgRequest>();
            var dtoResult = fixture.Create<DtoResult<Org>>();
            var mockCreateOrgService = new Mock<IMediator>();
            var mockAuthService = new Mock<IAuthorizationService>();
            dtoResult.AddError("test", "error message");

            mockCreateOrgService.Setup(s => s.Send(orgDto, _token)).ReturnsAsync(dtoResult);
            var sut = new AdminController(
                mockCreateOrgService.Object,
                _context,
                mockAuthService.Object
                );
            var result = await sut.Post(orgDto, _token);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async void ReturnErrorsInBadRequestObject()
        {
            var fixture = new Fixture();
            var orgDto = fixture.Create<CreateOrgRequest>();
            var dtoResult = fixture.Create<DtoResult<Org>>();
            var mockCreateOrgService = new Mock<IMediator>();
            var mockAuthService = new Mock<IAuthorizationService>();
            dtoResult.AddError("test", "error message");

            mockCreateOrgService.Setup(s => s.Send(orgDto, _token)).ReturnsAsync(dtoResult);

            var sut = new AdminController(
                mockCreateOrgService.Object,
                _context,
                mockAuthService.Object
                );
            var response = await sut.Post(orgDto, _token);
            var result = response as BadRequestObjectResult;

            var errorString = JsonConvert.SerializeObject(result?.Value);
            var errors = JObject.Parse(errorString);
            Assert.Equal("error message", errors["test"][0]);
        }
    }
}
