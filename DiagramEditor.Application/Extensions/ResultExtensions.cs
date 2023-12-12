using CSharpFunctionalExtensions;

namespace DiagramEditor.Application.Extensions;

public static class ResultExtensions
{
    public static Result<T, Unit> AsSuccess<T>(this T success)
    {
        return Result.Success<T, Unit>(success);
    }

    public static Result<T, E> AsSuccess<T, E>(this T success)
    {
        return Result.Success<T, E>(success);
    }

    public static Result<Unit, E> AsFailure<E>(this E error)
    {
        return Result.Failure<Unit, E>(error);
    }

    public static Result<T, E> AsFailure<T, E>(this E error)
    {
        return Result.Failure<T, E>(error);
    }
}
