using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VkCore.Models;
using VkCore.Requests.User;
using VkInfrastructure.RequestHandlers.User;
using Xunit;

namespace VkInfrastructure.Test.Requests.User
{
    public class UserLoginRequestHandlerShould
    {
        readonly CancellationToken _token;

        public UserLoginRequestHandlerShould()
        {
            _token = new CancellationToken();
        }

        [Fact]
        public async Task ReturnProperlyConfiguredJwtAsyncWhenEmailConfirmed()
        {
            var fixture = new Fixture();
            fixture.Customize(new AutoMoqCustomization());

            var credModel = fixture.Create<UserLoginRequest>();
            var vkUser = fixture.Create<VkUser>();

            var userManagerMock = DbContextBuilder.GetMockUserManager();
            userManagerMock.Setup(foo => foo.FindByNameAsync(credModel.UserName)).ReturnsAsync(vkUser);
            userManagerMock.Setup(foo => foo.CheckPasswordAsync(vkUser, credModel.Password)).ReturnsAsync(true);
            userManagerMock.Setup(foo => foo.IsEmailConfirmedAsync(vkUser)).ReturnsAsync(true);

            var sut = new UserLoginRequestHandler(userManagerMock.Object, DbContextBuilder.GetJwtFactory());
            var result = await sut.Handle(credModel, _token);

            var values = JsonConvert.DeserializeObject<Dictionary<string, string>>(result.Value);
            Assert.Equal(vkUser.Id, values["id"]);
            Assert.Equal("7200", values["expires_in"]);
        }

        [Fact]
        public async Task ReturnErrorWhenEmailIsNotConfirmed()
        {
            var fixture = new Fixture();
            fixture.Customize(new AutoMoqCustomization());

            var credModel = fixture.Create<UserLoginRequest>();
            var vkUser = fixture.Create<VkUser>();

            var userManagerMock = DbContextBuilder.GetMockUserManager();
            userManagerMock.Setup(foo => foo.FindByNameAsync(credModel.UserName)).ReturnsAsync(vkUser);
            userManagerMock.Setup(foo => foo.IsEmailConfirmedAsync(vkUser)).ReturnsAsync(false);

            var sut = new UserLoginRequestHandler(userManagerMock.Object, DbContextBuilder.GetJwtFactory());
            var result = await sut.Handle(credModel, _token);

            var actual = result.GetErrors()[string.Empty].First();
            Assert.Contains("Please confirm your email first. Check your email for a confirmation link.", actual);           
        }
    }
}
