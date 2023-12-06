using System.Text.RegularExpressions;
using DiagramEditor.Application.Attributes;
using DiagramEditor.Application.Services.User;
using Microsoft.Extensions.DependencyInjection;

namespace DiagramEditor.Infrastructure.Services.User;

[Injectable(ServiceLifetime.Singleton)]
internal sealed partial class BasicLoginValidator : ILoginValidator
{
    [GeneratedRegex(@"^[a-zA-Z_]{3..}")]
    private static partial Regex LoginRegex();

    public bool Validate(string login)
    {
        return LoginRegex().IsMatch(login);
    }
}
