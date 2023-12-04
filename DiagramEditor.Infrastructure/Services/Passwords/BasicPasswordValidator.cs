using System.Text.RegularExpressions;
using DiagramEditor.Application.Attributes;
using DiagramEditor.Application.Services.Passwords;
using Microsoft.Extensions.DependencyInjection;

namespace DiagramEditor.Infrastructure;

[Injectable(ServiceLifetime.Singleton)]
internal partial class BasicPasswordValidator : IPasswordValidator
{
    public bool Validate(string passwordText)
    {
        return passwordText.Length >= 8 && NumberRegex().Count(passwordText) >= 1;
    }

    [GeneratedRegex(@"\d")]
    private static partial Regex NumberRegex();
}
