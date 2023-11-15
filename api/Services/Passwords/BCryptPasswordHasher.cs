namespace DiagramEditor.Services.Passwords;

using System.Text.RegularExpressions;
using BCrypt.Net;
using DiagramEditor.Attributes;

[Injectable(ServiceLifetime.Singleton)]
public sealed class BCryptPasswordHasher : IPasswordHasher
{
    public string Hash(string passwordText)
    {
        return BCrypt.HashPassword(passwordText);
    }

    public bool Verify(string passwordText, string passwordHash)
    {
        return BCrypt.Verify(passwordText, passwordHash);
    }

    public bool Validate(string passwordText)
    {
        return passwordText.Length >= 8 && Regex.Count(passwordText, @"\d") >= 1;
    }
}
