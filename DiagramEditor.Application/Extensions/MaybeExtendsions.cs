using CSharpFunctionalExtensions;

namespace DiagramEditor.Application.Extensions;

public static class MaybeExtensions
{
    public static T? AsNullable<T>(this Maybe<T> maybe)
    {
        return maybe.HasValue ? maybe.Value : default;
    }

    public static Result<T, E> ToSuccess<T, E>(this Maybe<T> successMaybe, E error)
    {
        return successMaybe.HasValue ? successMaybe.Value : error;
    }

    public static Result<T, Unit> ToSuccess<T>(this Maybe<T> successMaybe)
    {
        return successMaybe.ToSuccess(Unit.Instance);
    }

    public static Result<T, E> ToFailure<T, E>(this Maybe<E> errorMaybe, T success)
    {
        return errorMaybe.HasValue ? errorMaybe.Value : success;
    }

    public static Result<Unit, E> ToFailure<E>(this Maybe<E> errorMaybe)
    {
        return errorMaybe.ToFailure(Unit.Instance);
    }

    public static Maybe<T> Tap<T>(this Maybe<T> maybe, Action<T> sideEffect)
    {
        maybe.Execute(sideEffect);
        return maybe;
    }

    public static Maybe<T> Tap<T, U>(this Maybe<T> maybe, Func<T, U> sideEffect)
    {
        maybe.Execute(value => _ = sideEffect(value));
        return maybe;
    }
}
