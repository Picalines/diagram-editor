using DiagramEditor.Application.Attributes;
using DiagramEditor.Application.Services.Users;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace DiagramEditor.Infrastructure.Services.Users;

[Injectable(ServiceLifetime.Singleton)]
internal sealed partial class BasicLoginValidator : ApplicationValidator<string>, ILoginValidator
{
    public BasicLoginValidator()
    {
        RuleFor(login => login)
            .MinimumLength(3)
            .WithMessage("login must be at least 3 characters long");

        RuleFor(login => login)
            .Matches(@"^[a-zA-Z0-9_]+$")
            .WithMessage("login can consist only of lating letters, numbers and '_'");
    }
}
