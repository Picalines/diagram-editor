using CSharpFunctionalExtensions;

namespace DiagramEditor.Application.Extensions;

public static class MaybeExtensions
{
    public static Result<T, Unit> ToSuccess<T>(this Maybe<T> successMaybe)
    {
        return successMaybe
            .Map(value => value.AsSuccess())
            .GetValueOrDefault(Result.Failure<T, Unit>(Unit.Instance));
    }

    public static Result<Unit, E> ToFailure<E>(this Maybe<E> errorMaybe)
    {
        return errorMaybe
            .Map(error => error.AsFailure())
            .GetValueOrDefault(Result.Success<Unit, E>(Unit.Instance));
    }
}
