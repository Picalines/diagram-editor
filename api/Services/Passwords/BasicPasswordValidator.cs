using System.Text.RegularExpressions;
using DiagramEditor.Attributes;

namespace DiagramEditor.Services.Passwords;

[Injectable(ServiceLifetime.Singleton)]
public sealed class BasicPasswordValidator : IPasswordValidator
{
    public bool Validate(string passwordText)
    {
        return passwordText.Length >= 8 && Regex.Count(passwordText, @"\d") >= 1;
    }
}
