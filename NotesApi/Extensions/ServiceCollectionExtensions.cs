using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using NotesApi.Infrastacture.Builders;
using NotesApi.Infrastacture.Interfaces;
using NotesApi.Infrastacture.Services;
using NotesApi.Persistence.Seeders;
using System.Text;

namespace NotesApi.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static void AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddTransient<ICurrentUserService, CurrentUserService>()
                .AddTransient<IResolveTagsService, ResolveTagsService>()
                .AddTransient<ITagBuilder, TagBuilder>()
                .AddSingleton<UserSeeder>()
                .AddSingleton<TagSeeder>();
        }

        public static void ConfigureSecurity(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(builder =>
            {
                var key = Encoding.UTF8.GetBytes(configuration.GetSection("JWTConfiguration:SecretKey").Get<string>());
                builder.SaveToken = true;
                builder.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = configuration.GetSection("JWTConfiguration:LifeTimeEnabled").Get<bool>(),
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration.GetSection("JWTConfiguration:Issuer").Get<string>(),
                    ValidAudience = configuration.GetSection("JWTConfiguration:Audience").Get<string>(),
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });
            services.AddAuthorization();
        }
    }
}
