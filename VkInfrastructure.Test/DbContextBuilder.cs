using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Moq;
using System;
using System.Security.Claims;
using System.Text;
using VkCore.Models;
using VkInfrastructure.Auth;
using VkInfrastructure.Data;

namespace VkInfrastructure.Test
{
    public static class DbContextBuilder
    {
        public static JwtFactory GetJwtFactory()
        {
            var secretKey = "supersecretkeygreaterthan128bits";
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));

            var options = new JwtIssuerOptions();
            options.Issuer = "webApi";
            options.Audience = "http://localhost:5000/";
            options.SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            var jwtOptions = Options.Create(options);

            var identity = new ClaimsIdentity();
            identity.AddClaim(new Claim("id", "test"));

            return new JwtFactory(jwtOptions);
        }

        public static Mock<UserManager<VkUser>> GetMockUserManager()
        {
            var mockUserManager = new Mock<UserManager<VkUser>>(
                new Mock<IUserStore<VkUser>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<IPasswordHasher<VkUser>>().Object,
                new IUserValidator<VkUser>[0],
                new IPasswordValidator<VkUser>[0],
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<IServiceProvider>().Object,
                new Mock<ILogger<UserManager<VkUser>>>().Object);

            return mockUserManager;
        }

        public static VkDbContext GetContext()
        {
            var options = CreateNewContextOptions();
            var mockDispatcher = new Mock<IMediator>();

            return new VkDbContext(options, mockDispatcher.Object);
        }

        private static DbContextOptions<VkDbContext> CreateNewContextOptions()
        {
            // Create a fresh service provider, and therefore a fresh
            // InMemory database instance.
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            // Create a new options instance telling the context to use an
            // InMemory database and the new service provider.
            var builder = new DbContextOptionsBuilder<VkDbContext>();
            builder.UseInMemoryDatabase("vocabkitchen")
                   .UseInternalServiceProvider(serviceProvider);

            return builder.Options;
        }
    }
}
