using DiagramEditor.Application.Attributes;
using DiagramEditor.Application.Services.Passwords;
using Microsoft.Extensions.DependencyInjection;

namespace DiagramEditor.Infrastructure.Services.Passwords;

using BCrypt.Net;

[Injectable(ServiceLifetime.Singleton)]
internal class BCryptPasswordHasher : IPasswordHasher
{
    public string Hash(string passwordText)
    {
        return BCrypt.HashPassword(passwordText);
    }

    public bool Verify(string passwordText, string passwordHash)
    {
        return BCrypt.Verify(passwordText, passwordHash);
    }
}
