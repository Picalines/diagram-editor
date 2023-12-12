using DiagramEditor.Application.Attributes;
using DiagramEditor.Application.Services.Diagrams;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace DiagramEditor.Infrastructure.Services.Diagrams;

[Injectable(ServiceLifetime.Singleton)]
internal sealed class DiagramNameValidator : ApplicationValidator<string>, IDiagramNameValidator
{
    public DiagramNameValidator()
    {
        RuleFor(name => name)
            .MinimumLength(1)
            .When(name => name.Trim().Length >= 1)
            .WithMessage("diagram name must contain at least one character");
    }
}
