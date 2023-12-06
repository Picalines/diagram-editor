using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CSharpFunctionalExtensions;
using DiagramEditor.Application;
using DiagramEditor.Application.Attributes;
using DiagramEditor.Application.Extensions;
using DiagramEditor.Application.Repositories;
using DiagramEditor.Application.Services.Authentication;
using DiagramEditor.Application.Services.Passwords;
using DiagramEditor.Domain;
using DiagramEditor.Infrastructure.Configuration.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace DiagramEditor.Infrastructure.Services.Authentication;

using DiagramEditor.Domain.Users;

[Injectable(ServiceLifetime.Singleton)]
internal sealed class JwtAuthenticator(
    JwtConfigurationSection jwtConfiguration,
    IUserRepository users,
    IPasswordHasher passwordHasher,
    IRefreshTokenRepository refreshTokenCache,
    IHttpContextAccessor httpContextAccessor
) : IAuthenticator
{
    public Maybe<User> IdentifyUser(string login, string passwordText)
    {
        return users
            .GetByLogin(login)
            .Where(user => passwordHasher.Verify(passwordText, user.PasswordHash));
    }

    public AuthTokens Authenticate(User user)
    {
        var accessToken = GenerateToken(
            jwtConfiguration.AccessToken,
            [new Claim("id", user.Id.ToString())]
        );

        var refreshToken = GenerateToken(jwtConfiguration.RefreshToken);

        var refreshExpirationDate = DateTime
            .UtcNow
            .AddMinutes(jwtConfiguration.RefreshToken.ExpirationMinutes);

        refreshTokenCache.SetToken(user.Id, refreshToken, refreshExpirationDate);

        return new AuthTokens(accessToken, refreshToken);
    }

    public bool ValidateRefresh(User user, string refreshToken)
    {
        return refreshTokenCache.GetToken(user.Id).Where(refreshToken.Equals).HasValue
            && ValidateRefreshToken(refreshToken);
    }

    public void Deauthenticate(User user)
    {
        refreshTokenCache.DeleteToken(user.Id);
    }

    public Maybe<User> GetAuthenticatedUser()
    {
        return httpContextAccessor
            .HttpContext
            .AsMaybe()
            .Map(httpContext => httpContext.User)
            .Bind(GetUserByPrincipal);
    }

    public Maybe<User> IdentifyUser(AuthTokens authTokens)
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
            var tokenHandler = new JwtSecurityTokenHandler();
            principal = tokenHandler.ValidateToken(authTokens.AccessToken, validationParameters, out var securityToken);
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
            .Bind(idClaim => idClaim.Value.MaybeParse<Guid>())
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
            var tokenHandler = new JwtSecurityTokenHandler();
            tokenHandler.ValidateToken(refreshToken, validationParameters, out _);
        }
        catch
        {
            return false;
        }

        return true;
    }
}