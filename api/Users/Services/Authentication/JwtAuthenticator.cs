using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CSharpFunctionalExtensions;
using DiagramEditor.Attributes;
using DiagramEditor.Configuration;
using DiagramEditor.Database.Models;
using DiagramEditor.Extensions;
using DiagramEditor.Repositories;
using DiagramEditor.Repositories.Cache;
using DiagramEditor.Services.Passwords;
using Microsoft.IdentityModel.Tokens;

namespace DiagramEditor.Services.Authentication;

[Injectable(ServiceLifetime.Singleton)]
public sealed class JwtAuthenticator(
    JwtConfiguration jwtConfiguration,
    IUserRepository users,
    IPasswordHasher passwordHasher,
    IRefreshTokenCache refreshTokenCache,
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

        refreshTokenCache.SetToken(user, refreshToken, refreshExpirationDate);

        return (accessToken, refreshToken);
    }

    public bool ValidateRefresh(User user, string refreshToken)
    {
        return refreshTokenCache.GetToken(user).Where(refreshToken.Equals).HasValue
            && ValidateRefreshToken(refreshToken);
    }

    public void Deauthenticate(User user)
    {
        refreshTokenCache.DeleteToken(user);
    }

    public Maybe<User> GetAuthenticatedUser()
    {
        return httpContextAccessor
            .HttpContext
            .AsMaybe()
            .Map(httpContext => httpContext.User)
            .Bind(GetUserByPrincipal);
    }

    public Maybe<User> GetUserByAccessToken(string expiredAccessToken)
    {
        var signinKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtConfiguration.AccessToken.Secret)
        );

        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateLifetime = false,
            IssuerSigningKey = signinKey,
            ValidIssuer = jwtConfiguration.Issuer,
            ValidAudience = jwtConfiguration.Audience,
        };

        ClaimsPrincipal principal;

        try
        {
            principal = new JwtSecurityTokenHandler().ValidateToken(
                expiredAccessToken,
                validationParameters,
                out var securityToken
            );
        }
        catch
        {
            return Maybe.None;
        }

        return GetUserByPrincipal(principal);
    }

    private Maybe<User> GetUserByPrincipal(ClaimsPrincipal claims)
    {
        return claims
            .Claims
            .TryFirst(claim => claim.Type is "id")
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
            new JwtSecurityTokenHandler().ValidateToken(refreshToken, validationParameters, out _);
        }
        catch
        {
            return false;
        }

        return true;
    }
}
