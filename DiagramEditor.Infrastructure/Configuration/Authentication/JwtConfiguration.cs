using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace DiagramEditor.Infrastructure.Configuration.Authentication;

internal static class JwtConfiguration
{
    public static void AddJwtBearerServices(
        IServiceCollection services,
        IConfiguration configuration
    )
    {
        var jwtConfiguration =
            configuration.GetSection("Jwt").Get<JwtConfigurationSection>()
            ?? throw new InvalidOperationException("invalid Jwt configuration");

        services.AddSingleton(jwtConfiguration);

        services.AddAuthentication(ConfigureAuthentication).AddJwtBearer(ConfigureJwtBearer);

        static void ConfigureAuthentication(AuthenticationOptions options)
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }

        void ConfigureJwtBearer(JwtBearerOptions options)
        {
            var signinKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtConfiguration.AccessToken.Secret)
            );

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                IssuerSigningKey = signinKey,
                ValidIssuer = jwtConfiguration.Issuer,
                ValidAudience = jwtConfiguration.Audience,
                ClockSkew = TimeSpan.Zero,
            };
        }
    }
}
