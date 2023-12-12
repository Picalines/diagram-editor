using CSharpFunctionalExtensions;
using FluentValidation;

namespace DiagramEditor.Infrastructure.Services;

using DiagramEditor.Application.Services;

internal abstract class ApplicationValidator<T> : AbstractValidator<T>, IValidator<T>
{
    Maybe<ValidationError> IValidator<T>.Validate(T value)
    {
        return Validate(value)
            .AsMaybe()
            .Where(result => result.IsValid is false)
            .Map(
                result =>
                    new ValidationError(
                        result.Errors.Select(failure => failure.ErrorMessage).ToArray()
                    )
            );
    }
}
