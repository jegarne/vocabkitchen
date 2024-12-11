using AutoFixture;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using VkCore.Interfaces;
using VkCore.Models;
using VkCore.Requests.User;
using VkInfrastructure.RequestHandlers.User;
using Xunit;

namespace VkInfrastructure.Test.Requests.User
{
    public class CreateUserRequestHandlerShould
    {
        private readonly Mock<IUserService> _userService;
        private readonly Mock<IMediator> _mediator;
        private readonly CancellationToken _token;

        public CreateUserRequestHandlerShould()
        {
            _userService = new Mock<IUserService>();
            _mediator = new Mock<IMediator>();
            _token = new CancellationToken(false);
        }

        [Fact]
        public async void CallCreateUser()
        {
            var fixture = new Fixture();

            var request = fixture.Create<CreateUserRequest>();
            request.OrganizationName = null;
            _userService.Setup(m => m.CreateAsync(It.IsAny<VkUser>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(IdentityResult.Success);

            var user = fixture.Create<VkUser>();
            _userService.SetupSequence(m => m.FindByEmailAsync(request.Email))
                        .Returns(Task.FromResult<VkUser>(null))
                        .Returns(Task.FromResult(user));

            var sut = new CreateUserRequestHandler(_userService.Object, _mediator.Object);
            var result = await sut.Handle(request, _token);

            _userService.Verify(m => m.CreateAsync(It.IsAny<VkUser>(), It.IsAny<string>(), It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}
