using CSharpFunctionalExtensions;

namespace DiagramEditor.Application.Extensions;

public static class ResultExtensions
{
    public static Result<T, Unit> AsSuccess<T>(this T success)
    {
        return success;
    }

    public static Result<T, E> AsSuccess<T, E>(this T success)
    {
        return success;
    }

    public static Result<Unit, E> AsFailure<E>(this E error)
    {
        return error;
    }

    public static Result<T, E> AsFailure<T, E>(this E error)
    {
        return error;
    }

    public static Result<T, E> ToResult<T, E>(this bool statusFlag, T trueValue, E falseValue)
    {
        return statusFlag ? trueValue : falseValue;
    }
}
