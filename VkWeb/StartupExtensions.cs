using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using VkCore.Interfaces;
using VkCore.Models;
using VkInfrastructure.Auth;
using VkInfrastructure.Data;
using VkInfrastructure.DataSeeding;
using VkInfrastructure.Email;
using VkInfrastructure.Services;
using VkWeb.AuthorizationHandlers;
using VkWeb.DataProtection;


namespace VkWeb
{
    public static class StartupExtensions
    {
        public static void AddDomainDependencies(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IJwtFactory, JwtFactory>();
            services.AddSingleton<IEmailSender, AwsEmailSender>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IAuthorizationHandler, UserTypeHandler>();
            services.AddTransient<IAdminSeeder, AdminSeeder>();
            services.AddTransient<ITagService, TagService>();
        }

        public static Settings UseInMemoryDb(this IServiceCollection services, IConfiguration configuration)
        {
            string dbName = Guid.NewGuid().ToString();
            services.AddDbContext<VkDbContext>(options =>
                options.UseInMemoryDatabase(dbName));

            return services.ConfigureSettings(configuration);
        }

        public static Settings UseLocalPostgres(this IServiceCollection services, IConfiguration configuration)
        {
            var config = services.ConfigureSettings(configuration);

            services.AddEntityFrameworkNpgsql().AddDbContext<VkDbContext>(opt =>
                opt.UseNpgsql(config.LocalhostPostgres,
                    b => b.MigrationsAssembly("VkWeb"))
            );

            // Add a DbContext to store your Database Keys
            services.AddDbContext<MyKeysContext>(opt =>
                opt.UseNpgsql(config.LocalhostPostgres));

            return config;
        }

        public static Settings UseAws(this IServiceCollection services, IConfiguration configuration)
        {
            var config = services.ConfigureSettings(configuration);

            services.AddEntityFrameworkNpgsql().AddDbContext<VkDbContext>(opt =>
                opt.UseNpgsql(config.AmazonRdsPostgres,
                    b => b.MigrationsAssembly("VkWeb"))
            );

            // Add a DbContext to store your Database Keys
            services.AddDbContext<MyKeysContext>(opt =>
                opt.UseNpgsql(config.AmazonRdsPostgres));

            // using Microsoft.AspNetCore.DataProtection;
            services.AddDataProtection()
                .PersistKeysToDbContext<MyKeysContext>();

            return config;
        }


        public static Settings ConfigureSettings(this IServiceCollection services, IConfiguration configuration)
        {

            services.Configure<Settings>(configuration.GetSection("dev"));
            var config = configuration.GetSection("dev").Get<Settings>();
            services.AddSingleton<IEmailConfig>(config);
            services.AddSingleton<IConnectionStrings>(config);
            services.AddSingleton<IVkConfig>(config);

            return config;
        }

        public static void AddJwtAuthorization(this IServiceCollection services, IJwtConfig config)
        {
            var SecretKey = config.JwtSecretKey;
            var _signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecretKey));

            // Configure JwtIssuerOptions
            services.Configure<JwtIssuerOptions>(options =>
            {
                options.Issuer = config.JwtIssuer;
                options.Audience = config.JwtAudience;
                options.SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256);
            });

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = config.JwtIssuer,

                ValidateAudience = true,
                ValidAudience = config.JwtAudience,

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _signingKey,

                RequireExpirationTime = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(configureOptions =>
            {
                configureOptions.ClaimsIssuer = config.JwtIssuer;
                configureOptions.TokenValidationParameters = tokenValidationParameters;
                configureOptions.SaveToken = true;
            });

            // api user claim policy
            services.AddAuthorization(options =>
            {
                options.AddPolicy("ApiUser", policy => policy.RequireClaim(JwtConstants.Strings.JwtClaimIdentifiers.Rol, JwtConstants.Strings.JwtClaims.ApiAccess));
            });

            // add identity
            var builder = services.AddIdentityCore<VkUser>(o =>
            {
                // configure identity options
                o.Password.RequireDigit = false;
                o.Password.RequireLowercase = false;
                o.Password.RequireUppercase = false;
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequiredLength = 4;
                o.SignIn.RequireConfirmedEmail = true;
            });
            builder = new IdentityBuilder(builder.UserType, typeof(IdentityRole), builder.Services);
            builder.AddEntityFrameworkStores<VkDbContext>().AddDefaultTokenProviders();
        }
    }
}
