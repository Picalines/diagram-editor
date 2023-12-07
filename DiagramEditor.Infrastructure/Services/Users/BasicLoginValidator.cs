using System.Text.RegularExpressions;
using DiagramEditor.Application.Attributes;
using DiagramEditor.Application.Services.Users;
using Microsoft.Extensions.DependencyInjection;

namespace DiagramEditor.Infrastructure.Services.Users;

[Injectable(ServiceLifetime.Singleton)]
internal sealed partial class BasicLoginValidator : ILoginValidator
{
    [GeneratedRegex(@"^[a-zA-Z0-9_]{3,}")]
    private static partial Regex LoginRegex();

    public bool Validate(string login)
    {
        return LoginRegex().IsMatch(login);
    }
}
