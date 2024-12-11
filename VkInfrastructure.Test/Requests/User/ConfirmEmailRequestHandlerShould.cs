using AutoFixture;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using VkCore.Interfaces;
using VkCore.Models;
using VkCore.Requests.User;
using VkInfrastructure.RequestHandlers.User;
using Xunit;

namespace VkInfrastructure.Test.Requests.User
{
    public class ConfirmEmailRequestHandlerShould
    {
        [Fact]
        public async void ReturnErrorWhenEmailIsNotFound()
        {
            var fixture = new Fixture();
            var userService = new Mock<IUserService>();
            var emailSender = new Mock<IEmailSender>();
            var context = new Mock<IHttpContextAccessor>();

            var request = fixture.Create<SendConfirmationEmailRequest>();
            var token = fixture.Create<CancellationToken>();

            userService.Setup(m => m.FindByEmailAsync(request.Email)).ReturnsAsync((VkUser)null);

            var sut = new SendConfirmationEmailRequestHandler(userService.Object, emailSender.Object, context.Object);
            var result = await sut.Handle(request, token);

            Assert.Contains("Email not found.", result.GetErrors()[string.Empty]);
        }

        [Fact]
        [STAThread]
        public async void BuildConfirmationEmailCorrectly()
        {
            var fixture = new Fixture();
            var userService = new Mock<IUserService>();
            var emailSender = new Mock<IEmailSender>();
            var context = new Mock<IHttpContextAccessor>();

            var request = fixture.Create<SendConfirmationEmailRequest>();
            var token = fixture.Create<CancellationToken>();
            var vkUser = fixture.Create<VkUser>();

            var defaultContext = new DefaultHttpContext();
            defaultContext.Request.Scheme = "https";
            defaultContext.Request.Host = new HostString("foo.com");
            context.Setup(_ => _.HttpContext).Returns(defaultContext);
            userService.Setup(m => m.FindByEmailAsync(request.Email)).ReturnsAsync(vkUser);
            userService.Setup(m => m.GenerateEmailConfirmationTokenAsync(vkUser))
            .ReturnsAsync("CfDJ8Mr1fdEhOQNCqFMfVjsy+GxeWf8RqCbRaQgIHoI82Rk2r4lTJ/Nv+5YYXtSlrFJLgK4/cpmGOmLrinuv9MqQA+6EyiXnAbqw3qVdOPbn2sz90VghraPLnmuRtNkf7mLXAgDk3Fz1hHTlfvG+tMEWj3eLNnyDE0FdrAj4CkMK+nA9GkYnYELp1808jEa/nedlDww6LwzxRpWXTSb1BD8CpnhskYzc/AGaF7y1tGYwZV8bHs5KzPGY0a7AThBofJJR/Q==");

            string emailBody = null;
            emailSender.Setup(s => s.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(true))
             .Callback<string, string, string>((email, subject, body) => emailBody = body);

            var sut = new SendConfirmationEmailRequestHandler(userService.Object, emailSender.Object, context.Object);
            var result = await sut.Handle(request, token);

            Assert.NotEmpty(emailBody);
        }
    }
}
