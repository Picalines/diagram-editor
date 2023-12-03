using CSharpFunctionalExtensions;

namespace DiagramEditor.Common.Extensions;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

public static class ResultExtensions
{
    public static Results<RS, RE> ToTypedResult<T, RS, RE>(
        this Result<T> result,
        Func<T, RS> successResult,
        Func<string, RE> errorResult
    )
        where RS : IResult
        where RE : IResult
    {
        return result.IsSuccess ? successResult(result.Value) : errorResult(result.Error);
    }

    public static Results<RS, RE> ToTypedResult<T, E, RS, RE>(
        this Result<T, E> result,
        Func<T, RS> successResult,
        Func<E, RE> errorResult
    )
        where RS : IResult
        where RE : IResult
    {
        return result.IsSuccess ? successResult(result.Value) : errorResult(result.Error);
    }
}
