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
    public Maybe<User> Identify(string login, string passwordText)
    {
        return users
            .GetByLogin(login)
            .Where(user => passwordHasher.Verify(passwordText, user.PasswordHash));
    }

    public Maybe<(string AccessToken, string RefreshToken)> Authenticate(User user)
    {
        var accessToken = GenerateToken(
            jwtConfiguration.AccessToken,
            [ new Claim("id", user.Id.ToString()) ]
        );

        var refreshToken = GenerateToken(jwtConfiguration.RefreshToken);

        return (accessToken, refreshToken);
    }

    public Maybe<User> GetCurrentUser()
    {
        return httpContextAccessor
            .HttpContext
            .AsMaybe()
            .Map(httpContext => httpContext.User.Claims)
            .Bind(claims => claims.TryFirst(claim => claim.Type is "id"))
            .Bind(idClaim => idClaim.Value.MaybeParse<int>())
            .Bind(users.GetById);
    }

    private string GenerateToken(
        JwtTokenConfiguration tokenConfiguration,
        IEnumerable<Claim>? claims = null
    )
    {
        return new JwtSecurityTokenHandler().WriteToken(
            new JwtSecurityToken(
                issuer: jwtConfiguration.Issuer,
                audience: jwtConfiguration.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(tokenConfiguration.ExpirationMinutes),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenConfiguration.Secret)),
                    SecurityAlgorithms.HmacSha256
                )
            )
        );
    }
}
