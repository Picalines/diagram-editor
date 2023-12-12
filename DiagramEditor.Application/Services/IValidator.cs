using CSharpFunctionalExtensions;

namespace DiagramEditor.Application.Services;

public sealed record ValidationError(IReadOnlyList<string> Messages);

public interface IValidator<T>
{
    public Maybe<ValidationError> Validate(T value);
}
