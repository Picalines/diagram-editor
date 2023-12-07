using DiagramEditor.Application.Attributes;
using DiagramEditor.Application.Services.Passwords;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace DiagramEditor.Infrastructure.Services.Passwords;

[Injectable(ServiceLifetime.Singleton)]
internal partial class BasicPasswordValidator : ApplicationValidator<string>, IPasswordValidator
{
    public BasicPasswordValidator()
    {
        RuleFor(password => password)
            .MinimumLength(8)
            .WithMessage("password must be at least 8 characters long");

        RuleFor(password => password).Matches(@"\d").WithMessage("password must contain digits");
    }
}
