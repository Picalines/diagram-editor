using FluentValidation;

namespace DiagramEditor.Infrastructure.Services;

using System.Collections.Generic;
using DiagramEditor.Application.Services;

internal abstract class ApplicationValidator<T> : AbstractValidator<T>, IValidator<T>
{
    public bool Validate(T value, out IReadOnlyList<string> errors)
    {
        var result = Validate(value);

        errors = result.IsValid ? [] : result.Errors.Select(error => error.ErrorMessage).ToArray();

        return result.IsValid;
    }
}
