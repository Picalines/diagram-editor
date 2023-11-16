using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CSharpFunctionalExtensions;
using DiagramEditor.Attributes;
using DiagramEditor.Configuration;
using DiagramEditor.Database.Models;
using DiagramEditor.Extensions;
using DiagramEditor.Repositories;
using DiagramEditor.Services.Passwords;
using Microsoft.IdentityModel.Tokens;

namespace DiagramEditor.Services.Authentication;

[Injectable(ServiceLifetime.Singleton)]
public sealed class JwtAuthenticator(
    IHttpContextAccessor httpContextAccessor,
    JwtConfiguration jwtConfiguration,
    IUserRepository users,
    IPasswordHasher passwordHasher
) : IAuthenticator
{
    public Maybe<(User User, string Token)> Authenticate(string login, string passwordText)
    {
        return users
            .GetByLogin(login)
            .Where(user => passwordHasher.Verify(passwordText, user.PasswordHash))
            .Map(user => (user, GenerateToken(user)));
    }

    public Maybe<User> GetCurrentUser()
    {
        return httpContextAccessor
            .HttpContext.AsMaybe()
            .Map(httpContext => httpContext.User.Claims)
            .Bind(claims => claims.TryFirst(claim => claim.Type is "id"))
            .Bind(idClaim => idClaim.Value.MaybeParse<int>())
            .Bind(users.GetById);
    }

    private string GenerateToken(User user)
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
