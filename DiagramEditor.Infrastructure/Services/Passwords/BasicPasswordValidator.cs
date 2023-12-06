using System.Text.RegularExpressions;
using DiagramEditor.Application.Attributes;
using DiagramEditor.Application.Services.Passwords;
using Microsoft.Extensions.DependencyInjection;

namespace DiagramEditor.Infrastructure.Services.Passwords;

[Injectable(ServiceLifetime.Singleton)]
internal partial class BasicPasswordValidator : IPasswordValidator
{
    [GeneratedRegex(@"\d")]
    private static partial Regex NumberRegex();

    public bool Validate(string passwordText)
    {
        return passwordText.Length >= 8 && NumberRegex().Count(passwordText) >= 1;
    }
}
