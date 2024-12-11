using System;
using System.Collections.Generic;
using System.Text;
using AutoFixture;
using Microsoft.AspNetCore.Identity;
using Moq;
using VkCore.Extensions;
using VkCore.Models;
using VkInfrastructure.Extensions;
using Xunit;

namespace VkInfrastructure.Test.Extensions
{
    public class UserManagerExtensionsShould
    {
        [Fact]
        public async void GenerateAndConfirmUrlFriendlyEmailConfirmationToken()
        {
            var fixture = new Fixture();
            var vkUser = fixture.Create<VkUser>();
            var testToken = "123+456+q==";
            var identityResult = IdentityResult.Success;
            var sut = DbContextBuilder.GetMockUserManager();

            sut.Setup(m => m.GenerateEmailConfirmationTokenAsync(vkUser)).ReturnsAsync(testToken);
            sut.Setup(m => m.ConfirmEmailAsync(vkUser, testToken)).ReturnsAsync(identityResult);

            var token = await sut.Object.GenerateUrlFriendlyEmailConfirmationTokenAsync(vkUser);
            var result = await sut.Object.ConfirmUrlFriendlyEmailAsync(vkUser, token);

            sut.Verify(m => m.ConfirmEmailAsync(vkUser, testToken), Times.Once);
        }

        [Fact]
        public async void GenerateAndConfirmUrlFriendlyPasswordResetToken()
        {
            var fixture = new Fixture();
            var vkUser = fixture.Create<VkUser>();
            var testToken = "123+456+q==";
            var password = "password";
            var identityResult = IdentityResult.Success;
            var sut = DbContextBuilder.GetMockUserManager();

            sut.Setup(m => m.GeneratePasswordResetTokenAsync(vkUser)).ReturnsAsync(testToken);
            sut.Setup(m => m.ResetPasswordAsync(vkUser, testToken, password)).ReturnsAsync(identityResult);

            var token = await sut.Object.GenerateUrlFriendlyPasswordResetTokenAsync(vkUser);
            var result = await sut.Object.ResetUrlFriendlyPasswordAsync(vkUser, token, password);

            sut.Verify(m => m.ResetPasswordAsync(vkUser, testToken, password), Times.Once);
        }
    }
}
