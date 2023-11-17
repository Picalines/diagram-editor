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
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.Tokens;

namespace DiagramEditor.Services.Authentication;

[Injectable(ServiceLifetime.Singleton)]
public sealed class JwtAuthenticator(
    JwtConfiguration jwtConfiguration,
    IUserRepository users,
    IPasswordHasher passwordHasher,
    IDistributedCache cache,
    IHttpContextAccessor httpContextAccessor
) : IAuthenticator
{
    public Maybe<User> IdentifyUser(string login, string passwordText)
    {
        return users
            .GetByLogin(login)
            .Where(user => passwordHasher.Verify(passwordText, user.PasswordHash));
    }

    public (string AccessToken, string RefreshToken) Authenticate(User user)
    {
        var accessToken = GenerateToken(
            jwtConfiguration.AccessToken,
            [ new Claim("id", user.Id.ToString()) ]
        );

        var refreshToken = GenerateToken(jwtConfiguration.RefreshToken);

        var refreshExpirationDate = DateTime
            .UtcNow
            .AddMinutes(jwtConfiguration.RefreshToken.ExpirationMinutes);

        cache.SetString(
            user.Id.ToString(),
            refreshToken,
            new() { AbsoluteExpiration = refreshExpirationDate }
        );

        return (accessToken, refreshToken);
    }

    public bool ValidateRefresh(User user, string refreshToken)
    {
        return cache.GetString(user.Id.ToString()) == refreshToken
            && ValidateRefreshToken(refreshToken);
    }

    public void Deauthenticate(User user)
    {
        cache.Remove(user.Id.ToString());
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

    private bool ValidateRefreshToken(string refreshToken)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var signinKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtConfiguration.RefreshToken.Secret)
        );

        var validationParameters = new TokenValidationParameters
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

        try
        {
            tokenHandler.ValidateToken(refreshToken, validationParameters, out _);
        }
        catch (SecurityTokenMalformedException)
        {
            return false;
        }

        return true;
    }
}
