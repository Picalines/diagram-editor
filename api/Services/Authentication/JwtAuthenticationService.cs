using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DiagramEditor.Attributes;
using DiagramEditor.Configuration;
using DiagramEditor.Database.Models;
using Microsoft.IdentityModel.Tokens;

namespace DiagramEditor.Services.Authentication;

[Injectable(ServiceLifetime.Singleton)]
public sealed class JwtAuthenticationService(JwtConfiguration jwtConfiguration)
    : IAuthenticationService
{
    public string GenerateToken(User user)
    {
        var signingKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtConfiguration.SignInSecret)
        );

        var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        var claims = new Claim[] { new Claim("id", user.Id.ToString()), };

        return new JwtSecurityTokenHandler().WriteToken(
            new JwtSecurityToken(
                jwtConfiguration.Issuer,
                jwtConfiguration.Audience,
                claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: signingCredentials
            )
        );
    }
}
